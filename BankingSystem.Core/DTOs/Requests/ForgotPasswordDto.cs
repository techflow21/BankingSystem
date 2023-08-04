using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Core.DTOs.Requests
{
    /*public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }*/

    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? ClientURI { get; set; }
    }
}
