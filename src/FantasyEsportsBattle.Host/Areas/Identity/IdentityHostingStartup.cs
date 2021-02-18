using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(FantasyEsportsBattle.Host.Areas.Identity.IdentityHostingStartup))]
namespace FantasyEsportsBattle.Host.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}