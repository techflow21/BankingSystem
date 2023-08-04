using AutoMapper;
using BankingSystem.Core.Contracts;
using BankingSystem.Core.DTOs.Requests;
using BankingSystem.Core.DTOs.Responses;
using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;
using BankingSystem.Infrastructure.GenericRepository;
using BankingSystem.Infrastructure.JwtFeatures;
using BankingSystem.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace BankingSystem.Service.Implementations
{
    public class AccountsService : IAccountsService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Account> _accountRepository;
        private readonly IMapper _mapper;
        private readonly JwtHandler _jwtHandler;
        private readonly IEmailService _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageUploadService _imageUploadService;
        private readonly decimal _defaultAccountBalance;

        public AccountsService(UserManager<User> userManager, IMapper mapper, JwtHandler jwtHandler, IEmailService emailSender, IUnitOfWork unitOfWork, IImageUploadService imageUploadService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _emailSender = emailSender;
            _imageUploadService = imageUploadService;
            _defaultAccountBalance = 0;
            _unitOfWork = unitOfWork;
            _accountRepository = _unitOfWork.GetRepository<Account>();
        }

        public async Task<RegistrationResponseDto> RegisterUser(UserRegistrationDto userForRegistration)
        {
            /*if (userForRegistration == null || !ModelState.IsValid)
                return new RegistrationResponseDto { Errors = new List<string> { "Invalid request data." } };*/

            var user = _mapper.Map<User>(userForRegistration);

            user.IsActive = true;
            //user.IsEmailConfirmed = true;
            user.DateRegistered = DateTime.UtcNow;

            var account = new Account()
            {
                AccountNumber = GenerateAccountNumber(),
                AccountPin = GenerateAccountPin(),
                Balance = _defaultAccountBalance,
            };

            if (userForRegistration.AccountType == null || userForRegistration.AccountType.ToLower() == "savings")
            {
                account.AccountType = AccountType.Savings;
            }
            else if (userForRegistration.AccountType.ToLower() == "current")
            {
                account.AccountType = AccountType.Current;
            }
            else if (userForRegistration.AccountType.ToLower() == "investment")
            {
                account.AccountType = AccountType.Investment;
            }
            account.AccountType = AccountType.Savings;

            await _accountRepository.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            if (userForRegistration.Image != null)
            {
                //await _awsStorageService.SaveImageToAWSStorage(model.Image, fileName);
                var imageUrl = await _imageUploadService.PhotoUpload(userForRegistration.Image);
                user.ImageUrl = imageUrl;
            }


            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return new RegistrationResponseDto { Errors = errors.ToList() };
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", user.Email }
            };

            var callback = QueryHelpers.AddQueryString(userForRegistration.ClientURI, param);

            var message = new Message(new string[] { user.Email }, "Email Confirmation token", callback, null);
            await _emailSender.SendEmailAsync(message);

            await _userManager.AddToRoleAsync(user, "Viewer");

            return new RegistrationResponseDto { IsSuccessfulRegistered = true };
        }

        public async Task<AuthResponseDto> Login(UserAuthenticationDto userForAuthentication)
        {
            var user = await _userManager.FindByNameAsync(userForAuthentication.Email);
            if (user == null)
                return new AuthResponseDto { ErrorMessage = "Invalid Request" };

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return new AuthResponseDto { ErrorMessage = "Email is not confirmed" };

            if (!await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
            {
                await _userManager.AccessFailedAsync(user);

                if (await _userManager.IsLockedOutAsync(user))
                {
                    var content = $@"Your account is locked out. To reset the password click this link: {userForAuthentication.ClientURI}";
                    var message = new Message(new string[] { userForAuthentication.Email },
                        "Locked out account information", content, null);

                    await _emailSender.SendEmailAsync(message);

                    return new AuthResponseDto { ErrorMessage = "The account is locked out" };
                }

                return new AuthResponseDto { ErrorMessage = "Invalid Authentication" };
            }

            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                var authResponse = await GenerateOTPFor2StepVerification(user);
                return authResponse;
            }

            var token = await _jwtHandler.GenerateToken(user);

            await _userManager.ResetAccessFailedCountAsync(user);

            return new AuthResponseDto { IsAuthSuccessful = true, Token = token };
        }


        private async Task<AuthResponseDto> GenerateOTPFor2StepVerification(User user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))
            {
                return new AuthResponseDto { ErrorMessage = "Invalid 2-Step Verification Provider." };
            }

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var message = new Message(new string[] { user.Email }, "Authentication token", token, null);

            await _emailSender.SendEmailAsync(message);

            return new AuthResponseDto { Is2StepVerificationRequired = true, Provider = "Email" };
        }


        public async Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return new ForgotPasswordResponseDto { ErrorMessage = "Invalid Request" };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", forgotPasswordDto.Email }
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);
            var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);

            await _emailSender.SendEmailAsync(message);

            return new ForgotPasswordResponseDto { IsEmailSent = true };
        }


        public async Task<ResetPasswordResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            /*if (!ModelState.IsValid)
                return new ResetPasswordResponseDto { Errors = new List<string> { "Invalid request data." } };*/

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return new ResetPasswordResponseDto { Errors = new List<string> { "Invalid Request" } };

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);
                return new ResetPasswordResponseDto { Errors = errors.ToList() };
            }

            await _userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

            return new ResetPasswordResponseDto { IsPasswordReset = true };
        }


        public async Task<EmailConfirmationResponseDto> EmailConfirmation(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new EmailConfirmationResponseDto { ErrorMessage = "Invalid Email Confirmation Request" };

            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return new EmailConfirmationResponseDto { ErrorMessage = "Invalid Email Confirmation Request" };

            return new EmailConfirmationResponseDto { IsEmailConfirmed = true };
        }


        public async Task<TwoStepVerificationResponseDto> TwoStepVerification(TwoFactorDto twoFactorDto)
        {
            var user = await _userManager.FindByEmailAsync(twoFactorDto.Email);
            if (user == null)
                return new TwoStepVerificationResponseDto { ErrorMessage = "Invalid Request" };

            var validVerification = await _userManager.VerifyTwoFactorTokenAsync(user, twoFactorDto.Provider, twoFactorDto.Token);
            if (!validVerification)
                return new TwoStepVerificationResponseDto { ErrorMessage = "Invalid Token Verification" };

            var token = await _jwtHandler.GenerateToken(user);

            return new TwoStepVerificationResponseDto { IsAuthSuccessful = true, Token = token };
        }

        public async Task<ExternalLoginResponseDto> ExternalLogin(ExternalAuthRequest externalAuth)
        {
            var payload = await _jwtHandler.VerifyGoogleToken(externalAuth);
            if (payload == null)
                return new ExternalLoginResponseDto { ErrorMessage = "Invalid External Authentication." };

            var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new User { Email = payload.Email, UserName = payload.Email };
                    await _userManager.CreateAsync(user);

                    // Prepare and send an email for the email confirmation

                    await _userManager.AddToRoleAsync(user, "Viewer");
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }

            if (user == null)
                return new ExternalLoginResponseDto { ErrorMessage = "Invalid External Authentication." };

            // Check for the Locked out account

            var token = await _jwtHandler.GenerateToken(user);

            return new ExternalLoginResponseDto { Token = token, IsAuthSuccessful = true };
        }

        private string GenerateAccountNumber()
        {
            var bankPrefix = "300";
            //var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            var timestamp = DateTimeOffset.Now.Second.ToString();

            var random = new Random();
            var randomNumber = random.Next(100, 999).ToString();

            var accountNumber = bankPrefix + timestamp + randomNumber;
            return accountNumber;
        }

        private string GenerateAccountPin()
        {
            var random = new Random();
            var accountPin = random.Next(1000, 9999).ToString();
            return accountPin;
        }
    }
}
