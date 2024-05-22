using Autofac;
using Autofac.Core;
using AutoMapper;
using BLL.Common.Interfaces;
using BLL.Common.Mapping;
using BLL.Common.Repository;
using BLL.DTO;
using BLL.Validator;
using DAL.Context.Persistance;
using FluentValidation;
using System;
using System.Linq;
using FluentValidation.Mvc;
namespace BLL
{
    public class DependencyInjection
    {
        public static void RegisterDependencies(ContainerBuilder builder)
        {
            // Registration of DbContext
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerLifetimeScope();

            // BLL Dependencies Registration
            builder.RegisterType<AccountRepository>().As<IAccountRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<InstitutionRepository>().As<IInstitutionRepository>();
            builder.RegisterType<DocumentRepository>().As<IDocumentRepository>();
            builder.RegisterType<ProjectRepository>().As<IProjectRepository>();

            // AutoMapper Configuration and Registration
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            builder.RegisterInstance(mapperConfiguration.CreateMapper()).As<IMapper>().SingleInstance();

            
        }
    }
}
