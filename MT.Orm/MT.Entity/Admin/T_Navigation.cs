using MT.Orm.DBAttribute;
using System.Collections.Generic;

namespace Complex.Entity.Admin
{
    [Table(TableName = "T_Navigation", TableComment = "导航菜单")]
    public class T_Navigation : EntityBase
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Column(ColumnName = "NavTitle", Description = "菜单名称", MaxLength = "50")]
        public string NavTitle { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [Column(ColumnName = "Linkurl", Description = "链接地址", MaxLength = "500")]
        public string Linkurl { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Column(ColumnName = "Sortnum", Description = "排序")]
        public int Sortnum { get; set; }
        /// <summary>
        /// "图标CSS
        /// </summary>
        [Column(ColumnName = "iconCls", Description = "图标CSS")]
        public string iconCls { get; set; }
        /// <summary>
        /// 图标URL
        /// </summary>
        [Column(ColumnName = "iconUrl", Description = "图标URL")]
        public string iconUrl { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary> 
        [Column(ColumnName = "IsVisible", Description = "是否显示")]
        public bool IsVisible { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        [Column(ColumnName = "ParentID", Description = "父ID")]
        public int ParentID { get; set; }
        /// <summary>
        /// 菜单标识
        /// </summary>
        [Column(ColumnName = "NavTag", Description = "菜单标识")]
        public string NavTag { get; set; }
        /// <summary>
        /// 大图标路径
        /// </summary>
        [Column(ColumnName = "BigImageUrl", Description = "大图标路径")]
        public string BigImageUrl { get; set; }
        /// <summary>
        /// 是否为系统菜单
        /// </summary> 
        //[System.ComponentModel.DefaultValue(false)]
        [Column(ColumnName = "IsSys", Description = "是否为系统菜单")]
        public bool IsSys { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public IEnumerable<T_Navigation> children
        {
            get;
            set;
        }
        /// <summary>
        /// 所有按钮
        /// </summary>
        public List<string> AllButtonHtmlList
        {
            get;
            set;
        }
        /// <summary>
        ///已选按钮
        /// </summary>
        public List<string> OwnedBut 
        { 
            get; 
            set; 
        }
        /// <summary>
        ///按钮HTML
        /// </summary>
        public List<string> ButtonHtmlList
        { 
            get; 
            set; 
        }
        /// <summary>
        /// 供角色授权使用
        /// </summary>
        public List<string> Buttons 
        { 
            get; 
            set;
        }
    }
}
