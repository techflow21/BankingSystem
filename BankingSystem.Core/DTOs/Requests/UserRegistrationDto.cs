using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Core.DTOs.Requests
{
    public class UserRegistrationDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? MiddleName { get; set; }
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public string? HomeAddress { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? StateOfOrigin { get; set; }
        public string? LGA { get; set; }
        public string? AccountType { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
        public IFormFile? Image { get; set; }

        public string? ClientURI { get; set; }
    }
}
