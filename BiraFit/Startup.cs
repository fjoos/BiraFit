using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BiraFit.Startup))]

namespace BiraFit
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}