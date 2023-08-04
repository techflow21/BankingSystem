using BankingSystem.Core.Contracts;

namespace BankingSystem.Service.ServiceInterfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
