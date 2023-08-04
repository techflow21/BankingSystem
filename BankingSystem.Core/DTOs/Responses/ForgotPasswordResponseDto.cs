namespace BankingSystem.Core.DTOs.Responses
{
    public class ForgotPasswordResponseDto
    {
        public bool IsEmailSent { get; set; }
        public string ErrorMessage { get; set; }
    }
}
