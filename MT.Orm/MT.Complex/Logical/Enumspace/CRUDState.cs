﻿

using MT.Orm.DBAttribute;
namespace MT.Complex.Logical.Enumspace
{
    public enum CRUDState
    {
        /// <summary>
        /// 违反唯一约束添加已存在
        /// </summary>
        [Description("违反唯一约束！")]
        UniqueErro = -1,
        /// <summary>
        /// Mongodb异常！
        /// </summary>
        [Description("Mongodb异常！")]
        MongoErro = -2
    }
}