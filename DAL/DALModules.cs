using Autofac;
using BLL.Entities;
using DAL.Context;

namespace DAL
{
    public class DALModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Book>>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Product>>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Department>>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Category>>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Variant>>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Repository<Cart>>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Repository<CartItem>>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.Name.EndsWith("Finder"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
