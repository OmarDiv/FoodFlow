using Azure;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace FoodFlow.Services.Impelement
{
    public class UserService(UserManager<ApplicationUser> userManager,
        ApplicationDbContext applicationDbContext,
        IRoleService roleService,
        IEmailSenderService emailConfirmationService,
        ILogger<UserService> logger) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = applicationDbContext;
        private readonly IRoleService _roleService = roleService;
        private readonly IEmailSenderService _emailConfirmationService = emailConfirmationService;
        private readonly ILogger<UserService> _logger = logger;


        public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await (from u in _context.Users
                   join ur in _context.UserRoles
                   on u.Id equals ur.UserId
                   join r in _context.Roles
                   on ur.RoleId equals r.Id into roles
                   where !roles.Any(x => x.Name == DefaultRoles.Customer)
                   select new
                   {
                       u.Id,
                       u.FirstName,
                       u.LastName,
                       u.Email,
                       u.IsDisabled,
                       Roles = roles.Select(x => x.Name).ToList()
                   })
                    .GroupBy(u => new
                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        u.IsDisabled
                    }).Select(u => new UserResponse
                    (
                        u.Key.Id,
                        u.Key.FirstName,
                        u.Key.LastName,
                        u.Key.Email,
                        u.Key.IsDisabled,
                        u.SelectMany(u => u.Roles)
                    )).ToListAsync(cancellationToken);

        public async Task<Result<UserResponse>> GetAsync(string id)
        {
            if (await _userManager.FindByIdAsync(id) is not { } user)
                return Result.Failure<UserResponse>(UserErrors.UserNotFound);

            var userRoles = await _userManager.GetRolesAsync(user);
            var response = (user, userRoles).Adapt<UserResponse>();
            return Result.Success(response);

        }
        public async Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
            if (existingUser)
                return Result.Failure<UserResponse>(UserErrors.DuplicatedEmail);

            var allwoedRoles = await _roleService.GetAllRolesAsync(cancellationToken: cancellationToken);
            if (request.Roles.Except(allwoedRoles.Select(x => x.Name)).Any())
                return Result.Failure<UserResponse>(UserErrors.InvalidRoles);

            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, request.Roles);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                _logger.LogInformation("Confirmation code {Code}", code);
                await _emailConfirmationService.SendConfirmEmailCodeAndSetPasswordEmailAsync(user, code);

                var response = (user, request.Roles).Adapt<UserResponse>();

                return Result.Success(response);
            }

            var error = result.Errors.First();
            return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> UpdateAsync(string userId, UpdateUserRequest request, CancellationToken cancellationToken)
        {
            if (!await _userManager.Users.AnyAsync(x => x.Id == userId))
                return Result.Failure(UserErrors.UserNotFound);

            // التحقق من صحة الأدوار
            var allowedRoles = await _roleService.GetAllRolesAsync(cancellationToken: cancellationToken);
            if (request.Roles.Except(allowedRoles.Select(x => x.Name)).Any())
                return Result.Failure(UserErrors.InvalidRoles); // مش UserResponse!
            var user = await _userManager.FindByIdAsync(userId);
            var emailChanged = user!.Email != request.Email;
            // التحقق من البريد الإلكتروني إذا تم تغييره
            if (emailChanged)
            {
                var existingUser = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
                if (existingUser)
                    return Result.Failure(UserErrors.DuplicatedEmail);
                user.EmailConfirmed = false;
            }
            user = request.Adapt(user);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var removeRoles = await _context.UserRoles.Where(x => x.UserId == userId)
                     .ExecuteDeleteAsync(cancellationToken);
                await _userManager.AddToRolesAsync(user, request.Roles);
                if (emailChanged)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    _logger.LogInformation("Confirmation code {Code}", code);
                    await _emailConfirmationService.SendConfirmationEmailAsync(user, code);
                }
                return Result.Success();
            }

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ConfirmSendEmailCodeAndSetPassowrd(ConfirmEmailAndSetPasswordRequest request)
        {
            if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
                return Result.Failure(UserErrors.InvalidCredentials);
            if (user.EmailConfirmed || await _userManager.HasPasswordAsync(user))
                return Result.Failure(UserErrors.UserAccountAlreadySetup);
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                await _userManager.AddPasswordAsync(user, request.Password);
                return Result.Success();
            }

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ResendConfirmSendEmailCodeAndSetPassowrd(ResendConfirmEmailAndSetPasswordRequest request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Failure(UserErrors.InvalidCredentials);
            if (user.EmailConfirmed || await _userManager.HasPasswordAsync(user))
                return Result.Failure(UserErrors.UserAccountAlreadySetup);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Confirmation code {Code}", code);
            await _emailConfirmationService.SendConfirmEmailCodeAndSetPasswordEmailAsync(user, code);
            return Result.Success();
        }

        public async Task<Result<UserProfileResponse>> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.Users
                .Where(x => x.Id == userId)
                .ProjectToType<UserProfileResponse>()
                .SingleAsync();
            return Result.Success(user);
        }


        public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            //var user = await _userManager.FindByIdAsync(userId);
            //user = request.Adapt(user);
            //var result = await _userManager.UpdateAsync(user!);
            await _userManager.Users
                 .Where(x => x.Id == userId)
                 .ExecuteUpdateAsync(setters => setters
                     .SetProperty(x => x.FirstName, request.FirstName)
                     .SetProperty(x => x.LastName, request.LastName)
                     );

            return Result.Success();
        }
        public async Task<Result> SendChangeEmailCodeAsync(string userId, ChangeEmailRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null && existingUser.Id != userId)
            {
                return Result.Failure(UserErrors.DuplicatedEmail);
            }

            if (await _userManager.FindByIdAsync(userId) is not { } user)
                return Result.Failure(UserErrors.InvalidCredentials);

            var code = await _userManager.GenerateChangeEmailTokenAsync(user!, request.Email);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Change email code {Code}", code);
            await _emailConfirmationService.SendChangeEmailConfirmationAsync(user, request.Email, code);
            return Result.Success();
        }
        public async Task<Result> ConfirmChangeEmailAsync(ConfirmChangeEmailRequest request)
        {
            if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
                return Result.Failure(UserErrors.InvalidCredentials);
            var code = "";
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            }
            catch (FormatException)
            {
                return Result.Failure(UserErrors.InvaildCode);
            }
            var result = await _userManager.ChangeEmailAsync(user, request.NewEmail, code);
            if (result.Succeeded)
            {
                user.UserName = request.NewEmail;
                await _userManager.UpdateAsync(user);
                return Result.Success();
            }

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ReSendChangeEmailCodeAsync(string userid, ReSendChangeEmailCode request)
        {
            if (await _userManager.FindByIdAsync(userid) is not { } user)
                return Result.Success();
            var existingUser = await _userManager.FindByEmailAsync(request.NewEmail);
            if (existingUser != null && existingUser.Id != userid)
                return Result.Failure(UserErrors.DuplicatedEmail);
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Re-send change email code {Code}", code);
            await _emailConfirmationService.SendChangeEmailConfirmationAsync(user, request.NewEmail, code);
            return Result.Success();
        }
        public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);
            if (result.Succeeded)
                return Result.Success();
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ToggleStatusAsync(string userId, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByIdAsync(userId) is not { } user)
                return Result.Failure(UserErrors.UserNotFound);

            user.IsDisabled = !user.IsDisabled;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Result.Success();
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> UnlockAsync(string userId, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByIdAsync(userId) is not { } user)
                return Result.Failure(UserErrors.UserNotFound);


            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            if (result.Succeeded)
                return Result.Success();
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

    }
}
