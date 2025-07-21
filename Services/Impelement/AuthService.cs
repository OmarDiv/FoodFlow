using FoodFlow.Contracts.Authentication;
using FoodFlow.Helpers;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace FoodFlow.Services.Impelement
{
    public class AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider,
        ILogger<AuthService> logger, IEmailSender emailSender,
        IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly int _refreshTokenExpiryDays = 14;

        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
                return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials);

            //TODO : السطرين دول لو هنستخدم طريقه عمل اكونت ويسجل ف نفس الوقت (هنا احنا بنجمع الكود بدل التكرار) بس احنا بنستخدم طريقه تانيه حاليا
            //var authResponse = await GenerateAuthTokensAsync(user);
            //return Result.Success(authResponse);

            var (token, expirationIn) = _jwtProvider.GenerateToken(user);
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
            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);
            if (oldRefreshToken is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidUserOrRefershToken);

            oldRefreshToken.RevokedOn = DateTime.UtcNow;

            var (newToken, expirationIn) = _jwtProvider.GenerateToken(user);
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
            var existingEmail = _userManager.Users.Any(x => x.Email == request.Email);
            if (existingEmail)
                return Result.Failure(UserErrors.DuplicatedEmail);

            var user = request.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                _logger.LogInformation("Confirmtion Code {code}", code);

                await SendComfirmationEmailAsync(user, code);

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
                return Result.Success();
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ResendConfirmEmailAsync(ResendConfirmtionEmailRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Confirmtion Code {code}", code);

            await SendComfirmationEmailAsync(user, code);

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
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        //TODO :  لو هنستخدم طريقه عمل اكونت ويسجل ف نفس الوقت بس احنا بنستخدم طريقه تانيه حاليا الي هيا انه هياكد الايميل الي كتبه عشان يقدر يسجل بدل ما يسجل دايركت
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
        */
        public async Task SendComfirmationEmailAsync(ApplicationUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmtion", new Dictionary<string, string>
                        {
                            { "{{UserName}}", user.FirstName },
                            { "{{ConfirmLink}}", $"{origin}/auth/confirm-email?userId={user.Id}&code={code}" }
                        });
            await _emailSender.SendEmailAsync(user.Email!, "Food Flow Team", emailBody);
        }

    }
}
