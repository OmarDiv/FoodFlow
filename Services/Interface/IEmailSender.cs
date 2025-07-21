namespace FoodFlow.Services.Interface
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmalMessage);
    }
}
