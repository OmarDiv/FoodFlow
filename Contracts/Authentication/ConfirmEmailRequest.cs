namespace FoodFlow.Contracts.Authentication
{
    public record ConfirmEmailRequest(
        string userId,
        string code);
}
