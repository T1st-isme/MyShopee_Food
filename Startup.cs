using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Shopee_Food.Startup))]
namespace Shopee_Food
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
