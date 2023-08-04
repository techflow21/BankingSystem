using MimeKit;

namespace BankingSystem.Service.ServiceInterfaces
{
    public interface IMailKitEmailService
    {
        Task SendEmailAsync(MimeMessage message);
    }
}
