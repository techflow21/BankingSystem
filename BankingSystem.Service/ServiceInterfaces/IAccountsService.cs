using BankingSystem.Core.DTOs.Requests;
using BankingSystem.Core.DTOs.Responses;

namespace BankingSystem.Service.ServiceInterfaces
{

    public interface IAccountsService
    {
        Task<RegistrationResponseDto> RegisterUser(UserRegistrationDto userForRegistration);
        Task<AuthResponseDto> Login(UserAuthenticationDto userForAuthentication);
        Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        Task<ResetPasswordResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<EmailConfirmationResponseDto> EmailConfirmation(string email, string token);
        Task<TwoStepVerificationResponseDto> TwoStepVerification(TwoFactorDto twoFactorDto);
        Task<ExternalLoginResponseDto> ExternalLogin(ExternalAuthRequest externalAuth);
    }
}
