namespace FoodFlow.Contracts.Users
{
    public record ConfirmEmailAndSetPasswordRequest(string UserId, string Code, string Password);
}
