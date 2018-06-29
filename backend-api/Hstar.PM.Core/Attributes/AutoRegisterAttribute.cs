using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hstar.PM.Core.Attributes
{
    public class AutoRegisterAttribute : Attribute
    {
        public AutoRegisterAttribute(Type serviceType, ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
        }

        /// <summary>
        /// Service Type
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Lifetime
        /// </summary>
        public ServiceLifetime Lifetime { get; }
    }
}
