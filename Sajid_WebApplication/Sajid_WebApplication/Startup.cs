using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sajid_WebApplication.Startup))]
namespace Sajid_WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
