namespace FoodFlow.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
        //Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        //Task<AuthResponse> RefreshTokenAsync(string token, CancellationToken cancellationToken = default);
        //Task<bool> LogoutAsync(CancellationToken cancellationToken = default);
        //Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default);
        //Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default);
        //Task<bool> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);
    }
}

