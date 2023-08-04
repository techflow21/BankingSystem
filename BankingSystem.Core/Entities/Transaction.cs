using BankingSystem.Core.Enums;

namespace BankingSystem.Core.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string SenderAccount { get; set; }
        public string ReceiverAccount { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
