 
using System;

namespace Complex.Entity.Admin
{
    [Serializable]
 
    public partial class LiveInformation:EntityBase
    {
        public LiveInformation()
        { }
        #region Model
        private int? _doordigitalid;
        private string _percardid;
        private int _liveinformationid;
        private string _housenumber;
        private string _address;
        private int? _grid;
        private DateTime? _entrytime;
        private DateTime? _modifiedtime;
        private string _livestate;
        private int? _registrationstatus;
        private string _contactway;
        private string _addtype;
        private string _changetype;
        private DateTime? _temporarytodate;
        private string _sitesaretype;
        private string _temporarycausebigtype;
        private string _professionalbigtype;
        private string _temporarycausesmalltype;
        private string _professionalsmalltype;
        /// <summary>
        /// DoorDigital表外键
        /// </summary>
        public int? DoorDigitalID
        {
            set { _doordigitalid = value; }
            get { return _doordigitalid; }
        }
        /// <summary>
        /// 用户表外键 身份证
        /// </summary>
        public string PerCardID
        {
            set { _percardid = value; }
            get { return _percardid; }
        }
        /// <summary>
        /// 主键
        /// </summary>
        public int LiveInformationID
        {
            set { _liveinformationid = value; }
            get { return _liveinformationid; }
        }
        /// <summary>
        /// 门牌号
        /// </summary>
        public string HouseNumber
        {
            set { _housenumber = value; }
            get { return _housenumber; }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 网格员ID
        /// </summary>
        public int? Grid
        {
            set { _grid = value; }
            get { return _grid; }
        }
        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime? EntryTime
        {
            set { _entrytime = value; }
            get { return _entrytime; }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiedTime
        {
            set { _modifiedtime = value; }
            get { return _modifiedtime; }
        }
        /// <summary>
        /// 居住状态
        /// </summary>
        public string LiveState
        {
            set { _livestate = value; }
            get { return _livestate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RegistrationStatus
        {
            set { _registrationstatus = value; }
            get { return _registrationstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContactWay
        {
            set { _contactway = value; }
            get { return _contactway; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AddType
        {
            set { _addtype = value; }
            get { return _addtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ChangeType
        {
            set { _changetype = value; }
            get { return _changetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? TemporaryToDate
        {
            set { _temporarytodate = value; }
            get { return _temporarytodate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SitesAreType
        {
            set { _sitesaretype = value; }
            get { return _sitesaretype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TemporaryCauseBigType
        {
            set { _temporarycausebigtype = value; }
            get { return _temporarycausebigtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProfessionalBigType
        {
            set { _professionalbigtype = value; }
            get { return _professionalbigtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TemporaryCauseSmallType
        {
            set { _temporarycausesmalltype = value; }
            get { return _temporarycausesmalltype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProfessionalSmallType
        {
            set { _professionalsmalltype = value; }
            get { return _professionalsmalltype; }
        }
        #endregion Model

    }
}

