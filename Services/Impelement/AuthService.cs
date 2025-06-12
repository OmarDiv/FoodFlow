namespace FoodFlow.Services.Impelement
{
    public class AuthService(UserManager<ApplicationUser> userManager,IJwtProvider jwtProvider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return null;
            var ( token,  expirationIn)= _jwtProvider.GenerateToken(user);


            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
                return null;

            return new AuthResponse(
                 user.Id,
                 user.Email,
                 user.FirstName,
                 user.LastName,
                 token,
                 expirationIn
            );
        }
    }
}
