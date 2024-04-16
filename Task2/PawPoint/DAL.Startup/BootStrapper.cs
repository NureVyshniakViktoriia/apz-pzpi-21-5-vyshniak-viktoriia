using System.Reflection;
using Autofac;

namespace DAL.Startup;
public static class Bootstrapper
{
    public static void Bootstrap(ContainerBuilder builder)
    {
        RegisterRepositories(builder);
    }

    private static void RegisterRepositories(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(Assembly.Load("DAL"))
            .Where(x => x.Name.EndsWith("Repository") 
                || x.Name.Equals("UnitOfWork"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}
