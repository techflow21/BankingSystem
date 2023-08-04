namespace BankingSystem.Core.DTOs.Requests
{
    public class DepositRequest
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
