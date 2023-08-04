using System.ComponentModel.DataAnnotations;


namespace BankingSystem.Core.DTOs.Requests
{
    public class TwoFactorDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Provider { get; set; }
        [Required]
        public string? Token { get; set; }
    }
}
