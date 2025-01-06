namespace FestivalHoa.Properties.Constants
{
    public static class DefaultNameCollection
    {
        public const string USERS = "CR_USERS";
        public const string REFRESHTOKEN = "CR_REFRESHTOKEN";
        
        public const string DONVI = "CR_DONVI";
        public const string DIAGIOIHANHCHINH = "CR_DIAGIOIHANHCHINH";
        public const string LOGGING = "CR_LOGGING";
        public const string MENU = "CR_MENU";
        public const string MENU_CONG_DAN = "CR_MENU_CONG_DAN";
        public const string MODULE = "CR_MODULE";
        public const string UNIT_ROLE = "CR_UNIT_ROLE";

        public const string FILES = "CR_FILES";


        #region nghiepvu
        public const string DOANHNGHIEP = "NV_DOANHNGHIEP";
        public const string HOA = "NV_HOA";
        #endregion


        #region danhmuc
        public const string DM_LOAIHOA = "DM_LOAIHOA";
        public const string DM_DONVITINH = "DM_DONVITINH";
        #endregion

    }

    public static class ConfigurationDb
    {
        #region MONGODB 
        public const string MONGO_CONNECTION_STRING = "DbSettings:ConnectionString";
        public const string MONGO_DATABASE_NAME = "DbSettings:DatabaseName";
        #endregion

        #region POSTGRE
        public const string POSTGRE_CONNECTION = "DbSettings:PostgreConnection";
        #endregion                                                                                                      
    }
}