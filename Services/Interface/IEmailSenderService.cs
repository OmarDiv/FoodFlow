namespace FoodFlow.Services.Interface
{
    public interface IEmailSenderService
    {
        Task SendConfirmationEmailAsync(ApplicationUser user, string code);
        Task SendChangeEmailConfirmationAsync(ApplicationUser user, string newEmail, string code);
        Task SendResetPasswordConfirmationAsync(ApplicationUser user, string code);
        Task SendConfirmEmailCodeAndSetPasswordEmailAsync(ApplicationUser user, string code);
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
