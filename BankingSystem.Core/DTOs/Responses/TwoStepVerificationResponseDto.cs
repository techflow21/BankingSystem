namespace BankingSystem.Core.DTOs.Responses
{
    public class TwoStepVerificationResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
    }
}
