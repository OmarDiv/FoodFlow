namespace FoodFlow.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expirationIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
        string? ValidateToken(string token);
    }
}
