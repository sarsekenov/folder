using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DiplomServer.Startup))]

namespace DiplomServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR("/chat",new Microsoft.AspNet.SignalR.HubConfiguration());

        }

    }
}
