using MT.Common.Utility.ReflectorExt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Diagnostics;

namespace UnitTestProject_ICO_AOP
{


    /// <summary>
    ///这是 ILPropertyAccessorTest 的测试类，旨在
    ///包含所有 ILPropertyAccessorTest 单元测试
    ///</summary>
    [TestClass()]
    public class ILPropertyAccessorTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        public string Test
        {
            get;
            set;
        }
        /// <summary>
        ///SetValue 的测试
        ///</summary>
        [TestMethod()]
        public void SetValueTest()
        {
            PropertyInfo propertyInfo = typeof(ILPropertyAccessorTest).GetProperty("Test"); // TODO: 初始化为适当的值
            ILPropertyAccessor target = new ILPropertyAccessor(propertyInfo); // TODO: 初始化为适当的值
            object instance = this; // TODO: 初始化为适当的值
            object value = "123123"; // TODO: 初始化为适当的值
            Stopwatch ssss = new Stopwatch();


            ssss.Reset();
            ssss.Start();
            for ( int i = 0; i < 10000000; i++ )
            {
                propertyInfo.SetValue(instance, i + "");
            }
            ssss.Stop();
            var aaaa1 = ssss.Elapsed.ToString();
          
            ssss.Reset();
            ssss.Start();
           
            for ( int i = 0; i < 10000000; i++ )
            {
                target.SetValue(instance, i + "");
          
            }
            ssss.Stop();
            var aaaa = ssss.Elapsed.ToString();


        
          
            Assert.AreEqual(true, true);
        }
    }
}
