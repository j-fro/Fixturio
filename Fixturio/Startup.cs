using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fixturio.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace Fixturio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
