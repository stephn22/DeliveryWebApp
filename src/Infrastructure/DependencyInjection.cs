﻿using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security.Policies;
using DeliveryWebApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DeliveryWebApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies()
                    .UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("DeliveryWebApp.WebUI"));
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            //services.AddResponseCaching();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.MaxAge = TimeSpan.FromDays(30);
            });

            services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRole>()
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
                options.AddPolicy(PolicyName.IsAuthenticated, policy => policy.AddRequirements(new IsAuthenticated()));
                options.AddPolicy(PolicyName.IsRestaurateur, policy => policy.AddRequirements(new IsRestaurateur()));
                options.AddPolicy(PolicyName.IsRider, policy => policy.AddRequirements(new IsRider()));
                options.AddPolicy(PolicyName.IsDefault, policy => policy.AddRequirements(new IsDefault()));
                options.AddPolicy(PolicyName.IsCustomer, policy => policy.AddRequirements(new IsCustomer()));
            });

            services.AddSingleton<IAuthorizationHandler, IsAuthenticatedAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, IsRestaurateurAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, IsRiderAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, IsDefaultAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, IsCustomerAuthorizationHandler>();

            return services;
        }
    }
}