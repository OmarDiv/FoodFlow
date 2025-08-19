using FoodFlow.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace FoodFlow.Services.Impelement
{
    public class AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider,
        ILogger<AuthService> logger,
        IHttpContextAccessor httpContextAccessor,
        IEmailSenderService emailConfirmationService,
        ApplicationDbContext applicationDbContext) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailSenderService _emailConfirmationService = emailConfirmationService;
        private readonly ApplicationDbContext _context = applicationDbContext;
        private readonly int _refreshTokenExpiryDays = 14;

        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            var result = await _signInManager.PasswordSignInAsync(user, password, false, true);
            if (!result.Succeeded)
            {
                var error = result.IsNotAllowed
                    ? UserErrors.EmailNotConfirmed
                    : result.IsLockedOut
                    ? UserErrors.LockedUser
                    : UserErrors.InvalidCredentials;

                return Result.Failure<AuthResponse>(error);
            }
            //TODO : السطرين دول لو هنستخدم طريقه عمل اكونت ويسجل ف نفس الوقت (هنا احنا بنجمع الكود بدل التكرار) بس احنا بنستخدم طريقه تانيه حاليا
            //var authResponse = await GenerateAuthTokensAsync(user);
            //return Result.Success(authResponse);
            (IEnumerable<string> roles, IEnumerable<string> Permissions) = await GetUserRolesAndPermissions(user, cancellationToken);
            var (token, expirationIn) = _jwtProvider.GenerateToken(user, roles, Permissions);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration,

            });
            await _userManager.UpdateAsync(user);



            return Result.Success(new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expirationIn, refreshToken, refreshTokenExpiration));
        }

        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidUserOrRefershToken);
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidUserOrRefershToken);
            if (user.IsDisabled)
                return Result.Failure<AuthResponse>(UserErrors.DisabledUser);
            if (user.LockoutEnd > DateTime.UtcNow)
                return Result.Failure<AuthResponse>(UserErrors.LockedUser);
            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);
            if (oldRefreshToken is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidUserOrRefershToken);

            oldRefreshToken.RevokedOn = DateTime.UtcNow;

            (IEnumerable<string> roles, IEnumerable<string> Permissions) = await GetUserRolesAndPermissions(user, cancellationToken);
            var (newToken, expirationIn) = _jwtProvider.GenerateToken(user, roles, Permissions);
            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiration,
            });
            var isUpdated = await _userManager.UpdateAsync(user);
            if (!isUpdated.Succeeded)
                return Result.Failure<AuthResponse>(UserErrors.FailedToUpdateUser);

            return Result.Success(new AuthResponse(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
              newToken,
                expirationIn,
                newRefreshToken,
                refreshTokenExpiration
           ));
        }

        public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            var existingEmail = await _userManager.Users.AnyAsync(x => x.Email == request.Email);
            if (existingEmail)
                return Result.Failure(UserErrors.DuplicatedEmail);

            var user = request.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                _logger.LogInformation("Confirmtion Code {code}", code);

                await _emailConfirmationService.SendConfirmationEmailAsync(user, code);

                return Result.Success();

            }

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            if (await _userManager.FindByIdAsync(request.userId) is not { } user)
                return Result.Failure(UserErrors.InvaildCode);

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var code = request.code;
            _logger.LogInformation("Confirming email with code {Code}", code);

            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return Result.Failure(UserErrors.InvaildCode);
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                var role = await _userManager.GetRolesAsync(user);
                if (role is null)// ملوش أي رول
                {
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Customer);
                }
                return Result.Success();
            }

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ResendConfirmEmailAsync(ResendConfirmtionEmailRequest request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success(); // لأمان - لا نكشف وجود الإيميل أم لا

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Resending confirmation email to {Email} , {code}", request.Email, code);

            await _emailConfirmationService.SendConfirmationEmailAsync(user, code);

            return Result.Success();
        }
        public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure(UserErrors.InvalidUserOrRefershToken);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure(UserErrors.InvalidUserOrRefershToken);

            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);
            if (oldRefreshToken is null)
                return Result.Failure(UserErrors.InvalidUserOrRefershToken);

            oldRefreshToken.RevokedOn = DateTime.UtcNow;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return Result.Failure(UserErrors.FailedToUpdateUser);

            return Result.Success();
        }

        public async Task<Result> SendResetPasswordCodeAsync(string email)
        {
            if (await _userManager.FindByEmailAsync(email) is not { } user)
                return Result.Success();
            if (!user.EmailConfirmed)
                return Result.Failure(UserErrors.EmailNotConfirmed);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Reset password code {Code}", code);
            await _emailConfirmationService.SendResetPasswordConfirmationAsync(user, code);
            return Result.Success();
        }
        public async Task<Result> ConfirmResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result.Failure(UserErrors.InvaildCode);
            IdentityResult result;
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
            }
            catch (Exception)
            {
                result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
            }
            if (result.Succeeded)
                return Result.Success();
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));

        }
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));


        private async Task<(IEnumerable<string> roles, IEnumerable<string> Permissions)> GetUserRolesAndPermissions(ApplicationUser user, CancellationToken cancellationToken)
        {
            var userRoles = await _userManager.GetRolesAsync(user);


            var UserPermissions = await (from role in _context.Roles
                                         join permissions in _context.RoleClaims
                                         on role.Id equals permissions.RoleId
                                         where userRoles.Contains(role.Name!)
                                         select permissions.ClaimValue!)
                                         .Distinct().ToListAsync(cancellationToken);

            return (userRoles, UserPermissions);
        }

        // :  لو هنستخدم طريقه عمل اكونت ويسجل ف نفس الوقت بس احنا بنستخدم طريقه تانيه حاليا الي هيا انه هياكد الايميل الي كتبه عشان يقدر يسجل بدل ما يسجل دايركت
        /*private async Task<AuthResponse> GenerateAuthTokensAsync(ApplicationUser user)
        {
            var (token, expirationIn) = _jwtProvider.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration,
            });
            await _userManager.UpdateAsync(user);

            return new AuthResponse(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                token,
                expirationIn,
                refreshToken,
                refreshTokenExpiration
            );
        }

        //Onof Backedge for Error Handling 
        //public async Task<OneOf<AuthResponse, Error>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);

        //    if (user is null)
        //        return UserErrors.InvalidCredentials;

        //    var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

        //    if (!isValidPassword)
        //        return UserErrors.InvalidCredentials;

        //    var (token, expiresIn) = _jwtProvider.GenerateToken(user);
        //    var refreshToken = GenerateRefreshToken();
        //    var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        //    user.RefreshTokens.Add(new RefreshToken
        //    {
        //        Token = refreshToken,
        //        ExpiresOn = refreshTokenExpiration
        //    });

        //    await _userManager.UpdateAsync(user);

        //    return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);
        //}

        */

    }
}
