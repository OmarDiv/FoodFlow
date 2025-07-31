using FoodFlow.Helpers;
using FoodFlow.Settings;
using Hangfire;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;

namespace FoodFlow.Services.Impelement
{
    public class EmailConfirmationService(
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager,
        IOptions<MailSettings> options,
        ILogger<EmailConfirmationService> logger) : IEmailConfirmationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<EmailConfirmationService> _logger = logger;
        private readonly MailSettings _mailSettings = options.Value;

        public async Task SendConfirmationEmailAsync(ApplicationUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmtion", new Dictionary<string, string>
                        {
                            { "{{UserName}}", user.FirstName },
                            { "{{ConfirmLink}}", $"{origin}/auth/confirm-email?userId={user.Id}&code={code}" }
                        });
            BackgroundJob.Enqueue(() => SendEmailAsync(user.Email!, "✅Food Flow:Confirm Your Email", emailBody));

            await Task.CompletedTask;
        }
        public async Task SendChangeEmailConfirmationAsync(ApplicationUser user, string newEmail, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("ChangeEmailConfirmation", new Dictionary<string, string>
                         {
                             { "{{UserName}}", user.FirstName },
                             { "{{ConfirmLink}}", $"{origin}/auth/confirm-change-email?userId={user.Id}&code={code}&newEmail={newEmail}" }
                         });

            BackgroundJob.Enqueue(() => SendEmailAsync(newEmail, "✅Food Flow:Confirm Your New Email", emailBody));

            await Task.CompletedTask;
        }
        public async Task SendResetPasswordConfirmationAsync(ApplicationUser user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("ResetPasswordConfirmation", new Dictionary<string, string>
                            {
                                { "{{UserName}}", user.FirstName },
                                { "{{ResetLink}}", $"{origin}/auth/reset-password?userId={user.Id}&code={code}" }
                            });

            BackgroundJob.Enqueue(() => SendEmailAsync(user.Email!, "✅Food Flow:Reset Your Password", emailBody));

            await Task.CompletedTask;
        }
        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            if (await _userManager.FindByIdAsync(request.userId) is not { } user)
                return Result.Failure(UserErrors.InvaildCode);

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var code = request.code;
            _logger.LogInformation("Confirming email with code {Code}", code);
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return Result.Failure(UserErrors.InvaildCode);

            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Result.Success();
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ResendConfirmEmailAsync(ResendConfirmtionEmailRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Resending confirmation email to {Code}", code);

            await SendConfirmationEmailAsync(user, code);

            return Result.Success();
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = subject
            };

            message.To.Add(MailboxAddress.Parse(email));

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();


            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(message);
            smtp.Disconnect(true);
        }
    }
}

