using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Orm.Model
{
    public class TempColumnModel
    {
        public String TableName { get; set; }
        public String ColumnName { get; set; }
        public String IsNullable { get; set; }
        public String TypeName { get; set; }
        public String MaxLength { get; set; }
    }
}
