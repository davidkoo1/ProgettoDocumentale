using Autofac;
using AutoMapper;
using BLL.Common.Interfaces;
using BLL.Common.Mapping;
using BLL.Common.Repository;
using DAL.Context.Persistance;
using FluentValidation;
using System;
using System.Linq;

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

            // Создание и регистрация AutoMapper
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            builder.RegisterInstance(mapperConfiguration.CreateMapper()).As<IMapper>().SingleInstance();

            // Регистрация FluentValidation
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                   .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

        }
    }
}
