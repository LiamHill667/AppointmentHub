using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AppointmentHub.Startup))]
namespace AppointmentHub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
