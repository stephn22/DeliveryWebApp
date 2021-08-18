using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .WriteTo.File("Log/log-.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        // seed database with admin if not exists
                        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                        await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager, scope.ServiceProvider);

                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "An error occurred while seeding the database.");
                        throw;
                    }
                    finally
                    {
                        Log.CloseAndFlush();
                    }
                }

                await host.RunAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
