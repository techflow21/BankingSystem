using BankingSystem.Core.Enums;

namespace BankingSystem.Core.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string AccountPin { get; set; }
        public AccountType AccountType { get; set; }
        public AtmCard? AtmCard { get; set; }
    }
}
