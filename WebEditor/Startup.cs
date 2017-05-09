using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebEditor.Startup))]
namespace WebEditor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
