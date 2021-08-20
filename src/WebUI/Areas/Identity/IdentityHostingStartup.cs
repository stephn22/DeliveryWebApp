using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(DeliveryWebApp.WebUI.Areas.Identity.IdentityHostingStartup))]
namespace DeliveryWebApp.WebUI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}