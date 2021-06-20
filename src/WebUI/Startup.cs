using System.Globalization;
using DeliveryWebApp.Application;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Infrastructure;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.WebUI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeliveryWebApp.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            services.AddRazorPages().AddDataAnnotationsLocalization();

            services.AddLocalization(options =>
            {
                options.ResourcesPath = Configuration["ResourcePath"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var cultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("en")
            };

            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(cultures[0]),
                SupportedCultures = cultures,
                SupportedUICultures = cultures
            };

            app.UseRequestLocalization(options);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
