using BankingSystem.Infrastructure.DataContext;
using BankingSystem.Infrastructure.GenericRepository;
using BankingSystem.Service.ExternalServices;
using BankingSystem.Service.Implementations;
using BankingSystem.Service.ServiceInterfaces;


namespace BankingSystem.API.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

        public static void ConfigureServices(this IServiceCollection services)
        {
            /*services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;

                //opt.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                opt.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();*/

            //services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();
            services.AddTransient<IMailKitEmailService, MailKitEmailService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAccountsService, AccountsService>();
            services.AddScoped<IImageUploadService, ImageUploadService>();

            services.AddScoped<IBankOperationService, BankOperationService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMailKitEmailService, MailKitEmailService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IContactService, ContactService>();

            services.AddScoped<IReportService, ReportService>();
        }
    }
}
