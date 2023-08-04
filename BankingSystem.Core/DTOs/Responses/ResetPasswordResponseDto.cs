namespace BankingSystem.Core.DTOs.Responses
{
    public class ResetPasswordResponseDto
    {
        public bool IsPasswordReset { get; set; }
        public List<string> Errors { get; set; }
    }
}
