namespace BankingSystem.Core.DTOs.Responses
{
    public class EmailConfirmationResponseDto
    {
        public bool IsEmailConfirmed { get; set; }
        public string ErrorMessage { get; set; }
    }
}
