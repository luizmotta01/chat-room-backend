using System;
using System.Reflection;
using Autofac;
using MediatR;
using MottaDevelopments.MicroServices.Application.Behaviors;
using Module = Autofac.Module;

namespace MottaDevelopments.MicroServices.Application.Autofac.Modules
{
    public class MediatorModule : Module
    {
        private readonly string[] _assemblies;

        public MediatorModule(params string[] assemblies) => _assemblies = assemblies;

        protected override void Load(ContainerBuilder builder)
        {
            foreach (var assemblyName in _assemblies)
            {
                var assembly = AppDomain.CurrentDomain.Load(assemblyName);
                
                LoadModules(builder, assembly);
            }

            builder.Register<ServiceFactory>(context =>
            {
                var component = context.Resolve<IComponentContext>();
                
                return type => component.TryResolve(type, out var obj) ? obj : null;
            });

            builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        }

        private static void LoadModules(ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterType<Mediator>().As<IMediator>().AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(INotificationHandler<>));
        }
    }
}