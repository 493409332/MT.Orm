using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MT.ICO_AOP.ModelAOP.Factory
{
    public class DynamicModelGenerator
    {
        const string assemblyname = "MtAop.DynamicModel{0}Assembly";
        const string AssemblyFileName = "MtAop.DynamicModel{0}Assembly.dll";
        const string ModuleName = "MtAop.DynamicModel{0}Module";
        const string TypeNameFormat = "MtAop.DynamicModel{0}Type";

       
        private Type _interfaceType;
        private AssemblyBuilder _assemblyBuilder;
        private ModuleBuilder _moduleBuilder;
        private TypeBuilder _typeBuilder;
      

        /// <summary>
        /// 构造
        /// </summary> 
        /// <param name="interfaceType"></param>
        public DynamicModelGenerator(  Type interfaceType)
        { 
            _interfaceType = interfaceType;
 
        }

        // 构造程序集
        void BuildAssembly()
        {
            // 程序集名字
            AssemblyName assemblyName = new AssemblyName(string.Format(assemblyname, _interfaceType.Name));

            // 在当前的AppDomain中构造程序集
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName,
                AssemblyBuilderAccess.RunAndSave, System.AppDomain.CurrentDomain.BaseDirectory);
        }

        // 构造模块
        void BuildModule()
        {
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(string.Format(ModuleName, _interfaceType.Name), string.Format(AssemblyFileName, _interfaceType.Name));
        }

        // 构造类型
        void BuildType()
        {
            _typeBuilder = _moduleBuilder.DefineType(string.Format(TypeNameFormat, _interfaceType.Name),
                TypeAttributes.Public | TypeAttributes.Sealed);
            _typeBuilder.SetParent(_interfaceType);
             
        }
        // 构造字段
        void BuildField()
        { 
        }

        // 构造函数
        void BuildConstructor()
        { 
        }
        /// <summary>
        /// 遍历对象中所有方法
        /// </summary>
        void BuildMethods()
        {
            PropertyInfo[] propertyinfos = _interfaceType.GetProperties(System.Reflection.BindingFlags.Public | BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly
);
          //  MethodInfo[] openbasemethodInfos = _realProxyType.GetMethods(System.Reflection.BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetCustomAttributes(typeof(OpenBaseAttribute)).Count() > 0).ToArray();


            foreach ( PropertyInfo propertyinfo in propertyinfos )
            {
                BuildPropertyInfo(propertyinfo);
            }
        }

        private void BuildPropertyInfo(PropertyInfo propertyinfo)
        { 
            PropertyBuilder custNamePropBldr;
            MethodBuilder custNameGetPropMthdBldr;
            MethodBuilder custNameSetPropMthdBldr;
            MethodAttributes getSetAttr;
            ILGenerator custNameGetIL;
            ILGenerator custNameSetIL;

            // 属性Set和Get方法要一个专门的属性。这里设置为Public。
            getSetAttr =
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName| MethodAttributes.Virtual ;
            //定义字段。
            FieldBuilder customerNameBldr = _typeBuilder.DefineField("_"+propertyinfo.Name,
                                                                     propertyinfo.PropertyType,
                                                                      FieldAttributes.Private);
       
           // TypedReference.ToObject(propertyinfo.PropertyType.ReflectedType);
            //定义属性。
            //最后一个参数为null,因为属性没有参数。
            custNamePropBldr = _typeBuilder.DefineProperty(propertyinfo.Name,
                                                             PropertyAttributes.HasDefault,
                                                             propertyinfo.PropertyType,
                                                             null);
 
            //定义Get方法。
            custNameGetPropMthdBldr =
                _typeBuilder.DefineMethod("get_"+propertyinfo.Name,
                                           getSetAttr,
                                            propertyinfo.PropertyType,
                                           Type.EmptyTypes);

            custNameGetIL = custNameGetPropMthdBldr.GetILGenerator();
             
            custNameGetIL.Emit(OpCodes.Ldarg_0);
            //custNameGetIL.Emit(OpCodes.Ldfld, customerNameBldr);
            custNameGetIL.Emit(OpCodes.Ldfld, customerNameBldr);
            custNameGetIL.Emit(OpCodes.Ldc_I4_S,11);
            custNameGetIL.Emit(OpCodes.Add);
            
            custNameGetIL.Emit(OpCodes.Ret);
            

  //            IL_0000:  nop
  //IL_0001:  ldarg.0
  //IL_0002:  ldfld      int32 Test.EntityBase::aaa
  //IL_0007:  ldc.i4.s   11
  //IL_0009:  add
  //IL_000a:  stloc.0
  //IL_000b:  br.s       IL_000d
  //IL_000d:  ldloc.0
  //IL_000e:  ret

            //定义Set方法。
            custNameSetPropMthdBldr =
                _typeBuilder.DefineMethod("set_" + propertyinfo.Name,
                                           getSetAttr,
                                           null,
                                           new Type[] { propertyinfo.PropertyType});
            custNameSetIL = custNameSetPropMthdBldr.GetILGenerator();

             
            custNameSetIL.Emit(OpCodes.Ldarg_0);
            custNameSetIL.Emit(OpCodes.Ldarg_1);
            custNameSetIL.Emit(OpCodes.Stfld, customerNameBldr);
            custNameSetIL.Emit(OpCodes.Ret);
            
            custNamePropBldr.SetGetMethod(custNameGetPropMthdBldr);
            custNamePropBldr.SetSetMethod(custNameSetPropMthdBldr);

            
      
        }
        public Type GenerateType()
        {
            // 构造程序集
            BuildAssembly();
            // 构造模块
            BuildModule();
            // 构造类型
            BuildType();
            // 构造字段
            BuildField();
            // 构造函数
            BuildConstructor();
            // 构造方法
            BuildMethods();

            Type type = _typeBuilder.CreateType();
            // 将新建的类型保存在硬盘上（如果每次都动态生成，此步骤可省略）
            _assemblyBuilder.Save(string.Format(AssemblyFileName, _interfaceType.Name));
            return type;
        }

        public static object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
        public static T CreateProxy<T>()
        {
            var generator = new DynamicModelGenerator(typeof(T));
            Type type = generator.GenerateType();

            return (T) Activator.CreateInstance(type);
        }
    }
}
