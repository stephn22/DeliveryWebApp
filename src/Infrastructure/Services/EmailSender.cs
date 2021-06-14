using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using SendGrid;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;

namespace DeliveryWebApp.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        public IAuthMessageSenderOptions Options { get; }

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string msg)
        {
            await Execute(Options.Key, subject, msg, email);
        }

        private async Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("deliveryappdotnet@gmail.com", Options.User), // TODO email in secret
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
