namespace BankingSystem.Core.DTOs.Responses
{
    public class ExternalLoginResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
    }
}
