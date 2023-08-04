using Microsoft.AspNetCore.Identity;

namespace BankingSystem.Core.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? HomeAddress { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? StateOfOrigin { get; set; }
        public string? LGA { get; set; }
        public string? ImageUrl { get; set; }
        //public bool IsEmailConfirmed { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateRegistered { get; set; }

        public int? AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
