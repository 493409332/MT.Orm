using MT.Orm.DBAttribute;
using System;

namespace Complex.Entity
{
    public class EntityBase
    {
        public EntityBase()
        {

        }
        [Column(ColumnName = "ID", Description = "ID", NotNull = true, Identity = true)]
        public int ID
        {
            get;
            set;
        }
        [Column(ColumnName = "IsDelete", Description = "删除标识")]
        public bool IsDelete
        {
            get;
            set;
        }
        private Guid _guid = Guid.NewGuid();
        [Column(ColumnName = "GuID", Description = "GuID")]
        public Guid GuID
        {
            get
            {
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }
        private DateTime _datatime = DateTime.Now;
        [Column(ColumnName = "DataTime", Description = "时间")]
        public DateTime DataTime
        {
            get
            {
                return _datatime;
            }
            set
            {
                _datatime = value;
            }
        }

    }
}
