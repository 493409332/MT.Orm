
using MT.Orm.DBAttribute;


namespace Complex.Entity.Admin
{
    [Table(TableName = "T_Button", TableComment = "操作按钮")]
    public class T_Button : EntityBase
    {
        [Column(ColumnName = "ButtonText", Description = "按钮名称", MaxLength = "50")]
        public string ButtonText
        {
            get;
            set;
        }
        [Column(ColumnName = "Sortnum", Description = "排序")]
        public int Sortnum
        {
            get;
            set;
        }
        [Column(ColumnName = "iconCls", Description = "图标class", MaxLength = "50")]
        public string iconCls
        {
            get;
            set;
        }
        [Column(ColumnName = "IconUrl", Description = "图标路径", MaxLength = "200")]
        public string IconUrl
        {
            get;
            set;
        }
        [Column(ColumnName = "ButtonTag", Description = "按钮标识", MaxLength = "50")]
        public string ButtonTag
        {
            get;
            set;
        }
        [Column(ColumnName = "Remark", Description = "描述", MaxLength = "500", NotNull = true)]
        public string Remark
        {
            get;
            set;
        }
        [Column(ColumnName = "IsSys", Description = "是否为系统菜单",NotNull = true)]
        public bool IsSys 
        { 
            get; 
            set; 
        }
        [Column(ColumnName = "ButtonHtml", Description = "按钮HTML", MaxLength = "500", NotNull = true)]
        public string ButtonHtml
        {
            get
            {
                return string.Format("<a id=\"a_{0}\" style=\"float:left\" href=\"javascript:;\" plain=\"true\" class=\"easyui-linkbutton\" icon=\"{1}\" title=\"{2}\">{2}</a>", this.ButtonTag, this.iconCls, this.ButtonText);
            }
        }
    }

}
