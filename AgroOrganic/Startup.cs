using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AgroOrganic.Startup))]
namespace AgroOrganic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
