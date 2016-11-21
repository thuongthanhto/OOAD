using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebLinhKienMayTinh.Startup))]
namespace WebLinhKienMayTinh
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
