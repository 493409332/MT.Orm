using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MT.Complex.Repository;
using MT.Entity;
using MT.Complex.Logical.Test;
using MT.Complex.Logical.AdminNew.AopAttribute;
using MT.Complex.Logical.Test.AopAttribute;
using MT.ICO_AOP.ICO.Attribute;
namespace Bridge
{
    [ICOConfig("bridgings")]
    public static class bridgings : RepositoryBase<EntityTest>
    {
         //[Start, Start1, Ex, End]
        public  List<EntityTest> GetEntity()
        {
            ExecutiveSQL("aaa");
            return null;
        
        }
    }
}
