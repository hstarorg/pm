using Autofac;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Loader;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Core;
using Hstar.PM.Core.Attributes;

namespace Hstar.PM.Core.Helpers
{
    public static class IocHelper
    {
        public static IContainer ApplicationContainer { get; private set; }

        private static List<Type> GetApplicationTypes()
        {
            //所有程序集 和程序集下类型
            var deps = DependencyContext.Default;
            var typeList = new List<Type>();
            deps.CompileLibraries
                // 排除所有的系统程序集、Nuget下载包
                .Where(lib => !lib.Serviceable && lib.Type != "package")
                .ToList().ForEach(lib =>
                {
                    try
                    {
                        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                        typeList.AddRange(assembly.GetTypes().Where(type => type != null));
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Load deps error.", e);
                    }
                });
            return typeList;
        }

        public static IServiceProvider BuildServiceProvider(IServiceCollection services, params IModule[] modules)
        {
            var builder = new ContainerBuilder();
            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }

            var types = GetApplicationTypes();

            types.Select(x => x.GetCustomAttribute<AutoRegisterAttribute>(true))
                .Where(x => x != null)
                .GroupBy(x => x.Lifetime)
                .Select(g =>
             new
             {
                 Key = g.Key,
                 TypeList = g.ToList()
             })
             .ToList()
             .ForEach(item =>
             {
                 var findTypes = item.TypeList.Select(x => x.ServiceType).ToArray();
                 var registerBuilder = builder.RegisterTypes(findTypes)
                .AsImplementedInterfaces()
                .PropertiesAutowired();
                 switch (item.Key)
                 {
                     case ServiceLifetime.Scoped:
                         registerBuilder.InstancePerDependency();
                         break;
                     case ServiceLifetime.Singleton:
                         registerBuilder.SingleInstance();
                         break;
                     case ServiceLifetime.Transient:
                         registerBuilder.InstancePerLifetimeScope();
                         break;
                 }
             });

            builder.Populate(services);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        public static void DisposeContainer()
        {
            ApplicationContainer.Dispose();
        }
    }
}
