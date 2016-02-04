using Complex.Entity;
using MT.Orm.DBAttribute;
using System;

namespace MT.Entity
{
    public enum Hehe
    {
        //hehe1 ,
        //hehe2,
        //hehe3 

        hehe1 = 1,
        hehe2 = 2,
        hehe3 = 4

    }
    [Table(TableName = "Table11", TableComment = "表1的描述")]
    public class EntityTest:EntityBase
    {
        private Guid _guid;

        public EntityTest()
        { }
        [Column(ColumnName = "Column1", Description = "字段描述1", MaxLength = "200", NotNull = true)]
        public int Column1
        {
            get;
            set;
       
        }
        [Column(ColumnName = "Column2", Description = "字段描述2", Primary = false, NotNull = false)]
        public string Column2
        {
            get;// {return this.Column2;}

            set;
            //{
            //    _set.Add("Column1", value);
            //}
        }
        [Column(ColumnName = "Column3", Description = "ziduan3", NotNull = false)]
        public int Column3
        {
            get;
            set;
        }
        //[Column(ColumnName = "Column4", Description = "字段描述4")]
        public DateTime Column5
        {
            get;
            set;
        }
        //[Column(ColumnName = "ID", Description = "ID", Identity = true, BeNull = false)]
        //public int ID
        //{
        //    get;
        //    set;
        //}
        [Column(ColumnName = "Guid2")]
        public Guid Guid2
        {
            get { return Guid.NewGuid(); }
            set { _guid = value; }
        }
        [Column(ColumnName = "Column123123", Description = "12312312311", MaxLength = "200")]
        public string Column123123
        {
            get;
            set;
        }
        [Column(ColumnName = "Column3333", Description = "Column3333")]
        public int Column3333
        {
            get;
            set;
        }
        [Column(ColumnName = "Column4444", Description = "Column4444")]
        public int Column4444
        {
            get;
            set;
        }

        [Column(ColumnName = "aaaaaa", Description = "aaaaaa")]
        public DateTime aaaaaa
        {
            get;
            set;
        }
       [Column(ColumnName = "aaaaaa", Description = "aaaaaa")]
        public Hehe aaaaaa1
        {
            get;
            set;
        }

     
    }
}
