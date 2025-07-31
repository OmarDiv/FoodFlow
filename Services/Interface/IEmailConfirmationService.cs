namespace FoodFlow.Services.Interface
{
    public interface IEmailConfirmationService
    {
        Task SendConfirmationEmailAsync(ApplicationUser user, string code);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<Result> ResendConfirmEmailAsync(ResendConfirmtionEmailRequest request);
        Task SendEmailAsync(string email, string subject, string htmalMessage);
        Task SendResetPasswordConfirmationAsync(ApplicationUser user, string code);
        Task SendChangeEmailConfirmationAsync(ApplicationUser user, string newEmail, string code);
    }
}
