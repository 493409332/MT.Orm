using System;

namespace MT.Orm.Model
{
     public class TransactionHelp
    {
         public Action Commit { get; set; }
         public Action Rollback { get; set; }
    }
}
