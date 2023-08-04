using BankingSystem.Core.Enums;

namespace BankingSystem.Core.Entities
{
    public class AtmCard
    {
        public int Id { get; set; }
        public AtmCardType CardType {get; set;}
        public string AtmCardNumber { get; set; }
        public string AtmCardPin { get; set; }
        public string CVVNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
