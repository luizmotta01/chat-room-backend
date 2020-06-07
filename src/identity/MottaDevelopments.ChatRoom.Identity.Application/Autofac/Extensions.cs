using Autofac;
using MottaDevelopments.ChatRoom.Identity.Application.Autofac.Modules;
using MottaDevelopments.MicroServices.Application.Autofac.Modules;

namespace MottaDevelopments.ChatRoom.Identity.Application.Autofac
{
    public static class Extensions
    {
        public static void AddAutoFacModules(this ContainerBuilder builder)
        {
            builder.RegisterModule(new IdentityModule());
            
            builder.RegisterModule(new MediatorModule("MottaDevelopments.ChatRoom.Identity.Application",
                "MottaDevelopments.ChatRoom.Identity.Domain"));
        }
    }
}
