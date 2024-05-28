using Autofac.Integration.Mvc;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BLL.Common.Repository;
using BLL.Common.Interfaces;
using BLL;
using FluentValidation;
using BLL.DTO;
using BLL.Validator;
using BLL.UserDTOs;
using BLL.DTO.InstitutionDTOs;
using BLL.DTO.ProjectDTOs;
using BLL.DTO.DocumentDTOs;

namespace WebUI.App_Start
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Регистрация зависимостей BLL и DAL
            DependencyInjection.RegisterDependencies(builder);

            // Регистрация всех контроллеров в текущей сборке
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            

            var container = builder.Build();

            // Установка резолвера зависимостей для MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}