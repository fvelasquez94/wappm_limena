using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(wappm_limena.Startup))]
namespace wappm_limena
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
