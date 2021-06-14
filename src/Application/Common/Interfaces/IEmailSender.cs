using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        public IAuthMessageSenderOptions Options { get; }

        public Task SendEmailAsync(string email, string subject, string msg);
    }
}
