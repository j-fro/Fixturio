using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fixturio.Startup))]
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
