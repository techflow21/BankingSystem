using BankingSystem.Core.DTOs.Requests;


namespace BankingSystem.Service.ServiceInterfaces
{
    public interface IBankOperationService
    {
        Task<string> DepositAsync(DepositRequest request);
        Task<string> TransferAsync(TransferRequest request);
        Task<string> CheckBalanceAsync(string accountNumber);
        Task<string> WithdrawAsync(WithdrawRequest request);
    }
}
