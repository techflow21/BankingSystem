using BankingSystem.Core.DTOs.Requests;


namespace BankingSystem.Service.ServiceInterfaces
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(RegisterRequest request);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<string> LoginAsync(LoginRequest request);
        Task<string> LogoutAsync();
        Task<string> ForgotPasswordAsync(ForgotPasswordDto request);
    }
}
