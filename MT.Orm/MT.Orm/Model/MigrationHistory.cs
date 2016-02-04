using MT.Orm.DBAttribute;
using System;

namespace MT.Orm.Model
{
    [Table(TableName = "__MigrationHistory", TableComment = "历史记录表")]
    public class MigrationHistory
    {
        [Column(ColumnName = "ID", Description = "编号")]
        public int ID { get; set; }
        public string MigrationId { get; set; }
        public string ContextKey { get; set; }
        public string Model { get; set; }
        public DateTime MigrationTime { get; set; }

    }
}
