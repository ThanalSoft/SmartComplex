using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ThanalSoft.SmartComplex.Api.Startup))]

namespace ThanalSoft.SmartComplex.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
