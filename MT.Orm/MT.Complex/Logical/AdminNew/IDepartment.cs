using Complex.Entity.Admin;
using MT.ICO.Attribute;
using MT.ICO.Interface;
using System;
using System.Collections.Generic;

namespace Complex.Logical.Admin
{
    [ICO_AOPEnable(true)]
    public interface IDepartment : ITransientLifetimeManagerRegister, IBase<T_Department>
    {
        List<T_Department> GetTreeGrid(int ParentID);
        List<T_Department> GetDepartment(int ParentID);
        String GetDepartmentName(int DepartmentID);
        List<T_Department> GetAllNoReplace();
    }
}
