using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FakeTrello.Startup))]
namespace FakeTrello
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
