using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Financial.Startup))]
namespace Financial
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
