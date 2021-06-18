using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using DeliveryWebApp.Infrastructure.Security.Policies;
using DeliveryWebApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryWebApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("DeliveryWebApp.WebUI")));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();

            /***************************** EmailSender *********************************/

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(configuration);

            /********************************** OAuth **********************************/

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    var googleAuthNSection = configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                })
                .AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
                    microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
                });

            /********************************** Policies **********************************/

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyName.IsAdmin, policy => policy.AddRequirements(new IsAdmin()));
                options.AddPolicy(PolicyName.IsRestaurateur, policy => policy.AddRequirements(new IsRestaurateur()));
                options.AddPolicy(PolicyName.IsRider, policy => policy.AddRequirements(new IsRider()));
                options.AddPolicy(PolicyName.IsDefault, policy => policy.AddRequirements(new IsDefault()));
            });

            services.AddSingleton<IAuthorizationHandler, IsAdminAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, IsRestaurateurAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, IsRiderAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, IsDefaultAuthorizationHandler>();

            return services;
        }
    }
}