using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SorooshAcountingServer.Startup))]
namespace SorooshAcountingServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
