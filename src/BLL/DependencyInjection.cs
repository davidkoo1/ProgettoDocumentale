using Autofac;
using BLL.Common.Interfaces;
using BLL.Common.Repository;
using DAL.Context.Persistance;

namespace BLL
{

    public class DependencyInjectionConfig
    {
        public static void RegisterDependencies(ContainerBuilder builder)
        {
            // Регистрация DbContext
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerLifetimeScope();

            // Регистрация зависимостей BLL
            builder.RegisterType<AccountRepository>().As<IAccountRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
        }
    }
}
