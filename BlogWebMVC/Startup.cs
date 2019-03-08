using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlogWebMVC.Startup))]
namespace BlogWebMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
