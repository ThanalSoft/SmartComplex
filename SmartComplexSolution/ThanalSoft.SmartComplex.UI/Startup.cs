using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ThanalSoft.SmartComplex.UI.Startup))]
namespace ThanalSoft.SmartComplex.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
