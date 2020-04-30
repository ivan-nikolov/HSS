using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Hss.Web.Areas.Identity.IdentityHostingStartup))]

namespace Hss.Web.Areas.Identity
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
