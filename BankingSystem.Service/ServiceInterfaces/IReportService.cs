using BankingSystem.Core.Entities;


namespace BankingSystem.Service.ServiceInterfaces
{
    public interface IReportService
    {
        Task<int> GetTotalUsersAsync();
        Task<decimal> GetTotalDepositsAsync();
        Task<decimal> GetTotalWithdrawalsAsync();
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<IEnumerable<Transaction>> GetDepositTransactionsAsync();
        Task<IEnumerable<Transaction>> GetWithdrawalTransactionsAsync();
    }
}
