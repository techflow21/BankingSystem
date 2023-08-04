using BankingSystem.Core.DTOs.Requests;

namespace BankingSystem.Service.ServiceInterfaces
{
    public interface IContactService
    {
        Task SubmitContactForm(ContactRequest request);
    }
}
