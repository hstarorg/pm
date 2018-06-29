using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hstar.PM.Core.Extensions
{
    public static class ObjectExtension
    {
        private static readonly Dictionary<string, bool> dicMap = new Dictionary<string, bool>();
        private static IMapperConfigurationExpression mapperConfig;
        public static void Init()
        {
            Mapper.Initialize(cfg =>
            {
                mapperConfig = cfg;
            });
        }

        public static T ToMapperObject<T>(this object obj) where T : class
        {
            var sourceType = obj.GetType();
            var key = $"{nameof(sourceType)}_{nameof(T)}";
            if (!dicMap.ContainsKey(key))
            {
                //Mapper.Instance.ConfigurationProvider
                mapperConfig.CreateMap(sourceType, typeof(T));
                dicMap.Add(key, true);
            }
            return Mapper.Map<T>(obj);
        }
    }
};

