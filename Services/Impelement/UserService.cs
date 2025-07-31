using FoodFlow.Abstractions;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace FoodFlow.Services.Impelement
{
    public class UserService(UserManager<ApplicationUser> userManager, IEmailConfirmationService emailConfirmationService, ILogger<UserService> logger) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IEmailConfirmationService _emailConfirmationService = emailConfirmationService;
        private readonly ILogger<UserService> _logger = logger;

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
            if (existingUser != null)
            {
                var isConfirmed = await _userManager.IsEmailConfirmedAsync(existingUser);
                if (isConfirmed)
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
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var result = await _userManager.ChangeEmailAsync(user, request.NewEmail, code);
            user.UserName = request.NewEmail;
            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
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
    }
}
