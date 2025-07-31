namespace FoodFlow.Contracts.Users
{
    public record ConfirmChangeEmailRequest(
        string UserId,
        string Code,
        string NewEmail
    );
}
