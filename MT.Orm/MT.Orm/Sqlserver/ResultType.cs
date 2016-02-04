
namespace Complex.Sqlserver
{
    public enum ResultType
    {
        /// <summary>
        /// 呵呵
        /// </summary>
        CREATE_DATABASE_SUCCESS = -1,
        TABLE_NOT_EXIST = -2,
        CHECK_OK_CHANGED = -3,
        CHECK_FALSE = -4,
        DELETE_AND_REBUILD = -5,
        ONLY_ADD_COLUMNS = -6,
        CHANGE_LENGTH = -7

    }
}