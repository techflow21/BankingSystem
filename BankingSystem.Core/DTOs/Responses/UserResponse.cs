using Microsoft.AspNetCore.Http;

namespace BankingSystem.Core.DTOs.Responses
{
    public class UserResponse
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string? AccountNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? Image { get; set; }
    }
}
