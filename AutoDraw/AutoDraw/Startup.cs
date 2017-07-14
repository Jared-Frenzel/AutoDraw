using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutoDraw.Startup))]
namespace AutoDraw
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
