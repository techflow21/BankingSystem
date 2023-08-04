namespace BankingSystem.Core.DTOs.Responses
{
    public class RegistrationResponseDto
    {
        public bool IsSuccessfulRegistered { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
