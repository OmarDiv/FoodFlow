namespace FoodFlow.Services.Interface
{
    public interface IJwtProvider
    {
        (string token, int expirationIn) GenerateToken(ApplicationUser user);
        string? ValidateToken(string token);
    }
}
