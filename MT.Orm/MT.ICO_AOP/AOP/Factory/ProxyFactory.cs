﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.ICO_AOP.AOP.Factory
{
    public class ProxyFactory
    {
        public static T CreateProxy<T>(Type realProxyType) 
        {
            var generator = new DynamicProxyGenerator(realProxyType, typeof(T));
            Type type = generator.GenerateType();

            return (T) Activator.CreateInstance(type);
        }
    }
}
