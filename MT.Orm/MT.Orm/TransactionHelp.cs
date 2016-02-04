using System;

namespace MT.Orm
{
    public class TransactionHelp
    {
        public Action Commit { get; set; }
        public Action Rollback { get; set; }
        public object DispostSL { get; set; }

    }
}
