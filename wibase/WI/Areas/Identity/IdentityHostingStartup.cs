using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(WI.Areas.Identity.IdentityHostingStartup))]
namespace WI.Areas.Identity
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