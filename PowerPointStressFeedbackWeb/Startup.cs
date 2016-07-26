using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PowerPointStressFeedbackWeb.Startup))]
namespace PowerPointStressFeedbackWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
