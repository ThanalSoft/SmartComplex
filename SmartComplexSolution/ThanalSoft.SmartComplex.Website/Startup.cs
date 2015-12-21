using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ThanalSoft.SmartComplex.Website.Startup))]
namespace ThanalSoft.SmartComplex.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
