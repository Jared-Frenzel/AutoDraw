using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JFrenzel.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace JFrenzel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
