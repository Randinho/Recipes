using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Recipes.Areas.Identity.IdentityHostingStartup))]
namespace Recipes.Areas.Identity
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