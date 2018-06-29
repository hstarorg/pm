using Autofac.Core;
using Hstar.PM.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hstar.PM.Core.Extensions
{
    public static class IocExtension
    {
        public static IServiceProvider ToIocProvider(this IServiceCollection services, params IModule[] modules)
        {
            return IocHelper.BuildServiceProvider(services);
        }
    }
}
