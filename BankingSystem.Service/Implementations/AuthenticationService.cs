using AutoMapper;
using BankingSystem.Core.DTOs.Requests;
using BankingSystem.Core.Entities;
using BankingSystem.Core.Enums;
using BankingSystem.Infrastructure.GenericRepository;
using BankingSystem.Service.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using MimeKit;

namespace BankingSystem.Service.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<Account> _accountRepository;
        private readonly IJwtService _jwtService;
        private readonly IMailKitEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageUploadService _imageUploadService;
        private readonly decimal _defaultAccountBalance;

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, IJwtService jwtService, IMailKitEmailService emailService, IMapper mapper, IImageUploadService imageUploadService, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _emailService = emailService;
            _mapper = mapper;
            _imageUploadService = imageUploadService;
            _roleManager = roleManager;
            _defaultAccountBalance = 0;
            _unitOfWork = unitOfWork;
            _accountRepository = _unitOfWork.GetRepository<Account>();
        }

        public async Task<string> RegisterAsync(RegisterRequest request)
        {
            var userExists = await _userManager.FindByNameAsync(request.UserName.ToLower());
            if (userExists != null)
            {
                return "User already exists!";
            }

            var user = _mapper.Map<User>(request);
            user.IsActive = true;
            //user.IsEmailConfirmed = true;
            user.DateRegistered = DateTime.UtcNow;

            var account = new Account()
            {
                AccountNumber = GenerateAccountNumber(),
                AccountPin = GenerateAccountPin(),
                Balance = _defaultAccountBalance,
            };

            if(request.AccountType.ToLower() == "investment")
            {
                account.AccountType = AccountType.Investment;
            }
            else if(request.AccountType.ToLower() == "current")
            {
                account.AccountType = AccountType.Current;
            }
            account.AccountType = AccountType.Savings;

            await _accountRepository.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            if (request.Image != null)
            {
                //await _awsStorageService.SaveImageToAWSStorage(model.Image, fileName);
                var imageUrl = await _imageUploadService.PhotoUpload(request.Image);
                user.ImageUrl = imageUrl;
            }

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var userRoleExist = await _roleManager.RoleExistsAsync("user");

                if (!userRoleExist)
                {
                    var newUserRole = new IdentityRole { Name = "user" };
                    await _roleManager.CreateAsync(newUserRole);
                }
                await _userManager.AddToRoleAsync(user, "user");
                await _userManager.UpdateAsync(user);

                var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await SendConfirmationEmail(user, emailConfirmToken);

                return "User registered successfully. An email confirmation link has been sent to your email address.";
            }
            return "Unable to register user";
        }


        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return "Invalid email confirmation link.";
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "User not found.";
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return "Failed to confirm email.";
            }
            return "User's Account Confirmed, You can proceed to login...";
        }


        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return "Invalid email or password.";
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return "Email not confirmed. Please check your email for the confirmation link.";
            }
            var token = _jwtService.GenerateToken(user);
            return token;
        }

        public async Task<string> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return "User logged out successfully.";
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return "Invalid account";
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await SendForgotPasswordEmail(user, token);
            return "Password reset email sent.";
        }


        private async Task SendConfirmationEmail(User user, string emailConfirmationToken)
        {
            var confirmationLink = $"https://localhost.com:7184/api/authentication/confirm?userId={user.Id}&token={Uri.EscapeDataString(emailConfirmationToken)}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SOB-Banking System", "noreply@sobtech.com"));
            message.To.Add(new MailboxAddress(user.UserName, user.Email));
            message.Subject = "Account Confirmation";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Hello {user.UserName},</p><p>Please click the following link to confirm your account: <a href=\"{confirmationLink}\">Confirm Email</a></p>";

            message.Body = bodyBuilder.ToMessageBody();

            await _emailService.SendEmailAsync(message);
        }

        private async Task SendForgotPasswordEmail(User user, string forgotPasswordToken)
        {
            var forgotPasswordLink = $"https://localhost.com:7184/api/authentication/confirm?userId={user.Id}&token={Uri.EscapeDataString(forgotPasswordToken)}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SOB-Banking System", "noreply@sobtechnologies.com"));
            message.To.Add(new MailboxAddress(user.UserName, user.Email));
            message.Subject = "Forgot Password Link";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Hello {user.UserName},</p><p>Please click the following link to reset your account password: <a href=\"{forgotPasswordLink}\">Reset Password</a></p>";

            message.Body = bodyBuilder.ToMessageBody();
            await _emailService.SendEmailAsync(message);
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
