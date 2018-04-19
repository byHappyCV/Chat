using ChatWhitAuth.Hubs;
using ChatWhitAuth.Interfaces;
using ChatWhitAuth.IoC;
using ChatWhitAuth.Models;
using ChatWhitAuth.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChatWhitAuth.Startup))]
namespace ChatWhitAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(
                typeof(ChatHub),
                () => new ChatHub(new FriendManager(new UnitOfWork(new ApplicationDbContext()))));
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
