using Autofac;
using MottaDevelopments.MicroServices.Application.Autofac.Modules;

namespace MottaDevelopments.ChatRoom.Contacts.Application.Autofac
{
    public static class Extensions
    {
        public static void AddAutoFacModules(this ContainerBuilder builder)
        {
            builder.RegisterModule(new MediatorModule("MottaDevelopments.ChatRoom.Contacts.Application",
                "MottaDevelopments.ChatRoom.Contacts.Domain"));
        }
    }
}
