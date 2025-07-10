using FoodFlow.Const;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace FoodFlow.Services.Impelement
{
    public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly int _refreshTokenExpiryDays = 14;

        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

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
    }
}
