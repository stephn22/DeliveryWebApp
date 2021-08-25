using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using IEmailSender = Microsoft.AspNetCore.Identity.UI.Services.IEmailSender;

namespace DeliveryWebApp.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public AuthMessageSenderOptions Options { get; }

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, IConfiguration configuration)
        {
            Options = optionsAccessor.Value;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string msg)
        {
            await Execute(Options.SendGridKey, subject, msg, email);
        }

        private async Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_configuration["EmailSender:Email"], Options.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(email));
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }
    }
}
