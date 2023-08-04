using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Core.DTOs.Requests
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password") ]
        public string ConfirmPassword { get; set; }

        public string? HomeAddress { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? StateOfOrigin { get; set; }
        public string? LGA { get; set; }
        public string? AccountType { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
        public IFormFile? Image { get; set; }
    }
}
