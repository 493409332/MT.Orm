using System;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using MT.ICO_AOP.AOP.Factory;
using MT.ICO_AOP.ICO.Attribute;
using MT.ICO_AOP.ICO.Interface;

namespace MT.ICO_AOP.ICO.Factory
{
    public static class DependencyFactory
    {
        private static Type[] ILifetimeManagerRegisterList = new Type[] { 
                      typeof(ITransientLifetimeManagerRegister), 
                      typeof(IContainerControlledLifetimeManagerRegister),
                      typeof(IHierarchicalLifetimeManagerRegister),
                      typeof(IExternallyControlledLifetimeManagerRegister),
                      typeof(IPerThreadLifetimeManagerRegister),
                      typeof(IPerResolveLifetimeManagerRegister)};

        //   DependencyFactory.RegisterDependency(ProjectType.Wcf, "MT.Complex");
        public static void RegisterDependency(ProjectType projecttype, string projectname)
        {
            string DllPath = string.Empty;

            switch ( projecttype )
            {
                case ProjectType.Web:
                    DllPath = AppDomain.CurrentDomain.BaseDirectory + "\\bin\\" + projectname + ".dll";
                    break;
                case ProjectType.Winfom:
                case ProjectType.WPF:
                case ProjectType.Wcf:
                case ProjectType.Test:
                    DllPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + projectname + ".dll";
                    break;

            }
            if ( DllPath == null || DllPath.Length == 0 )
            {
                throw new Exception("无法解析项目DLL");
            }
            var typeList =
                  Assembly.LoadFrom(DllPath).GetTypes().Where(t => t.Namespace != null && t.Namespace.Contains("Realization") && t.IsInterface == false && t.IsAbstract == false);

            var LifetimeManagerRegisterlist =
             typeList.Where(t =>
             {
                 return t.GetInterfaces().Intersect(ILifetimeManagerRegisterList).Count() > 0;
             });

            foreach ( var t in LifetimeManagerRegisterlist )
            {
                var InterfaceList = t.GetInterfaces().Where(p =>
                { 
                    return !ILifetimeManagerRegisterList.Contains(p) && p.GetCustomAttribute(typeof(ICO_AOPEnableAttribute), false) != null;
                });
                LifetimeManager lifetimemanager = new TransientLifetimeManager();
                var intertype = t.GetInterfaces().Intersect(ILifetimeManagerRegisterList).First();
                switch ( intertype.Name )
                {
                    case "IContainerControlledLifetimeManagerRegister":
                        lifetimemanager = new ContainerControlledLifetimeManager();
                        break;
                    case "IHierarchicalLifetimeManagerRegister":
                        lifetimemanager = new HierarchicalLifetimeManager();
                        break;
                    case "IExternallyControlledLifetimeManagerRegister":
                        lifetimemanager = new ExternallyControlledLifetimeManager();
                        break;
                    case "IPerThreadLifetimeManagerRegister":
                        lifetimemanager = new PerThreadLifetimeManager();
                        break;
                    case "IPerResolveLifetimeManagerRegister":
                        lifetimemanager = new PerResolveLifetimeManager();
                        break;
                }

                foreach ( var iType in InterfaceList )
                {
                    ICOConfigAttribute ds = (ICOConfigAttribute) t.GetCustomAttribute(typeof(ICOConfigAttribute), false);
                    ICO_AOPEnableAttribute ia = (ICO_AOPEnableAttribute) iType.GetCustomAttribute(typeof(ICO_AOPEnableAttribute), false);
                     

                    if ( ia.AOPEnable )
                    {
                        var generator = new DynamicProxyGenerator(t, iType);
                        Type type = generator.GenerateType();

                      //  Type type = typeof(TransientLifetimeManager);
                        DependencyUnityContainer.Current.RegisterType(iType, type, ds.Description, lifetimemanager);
                    }
                    else
                    {
                        DependencyUnityContainer.Current.RegisterType(iType, t, ds.Description, lifetimemanager);
                    } 

                }
            }



        }


    }

}
