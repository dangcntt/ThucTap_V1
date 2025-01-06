using MongoDB.Driver;
using FestivalHoa.Properties.Constants;
using FestivalHoa.Properties.Models.Auth;
using FestivalHoa.Properties.Models.CongDan;
using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.Installers
{
    public class DataContext
    {
      

        #region Auth 
        private readonly IMongoCollection<RefreshTokenModel> _refreshToken;
        private readonly IMongoCollection<User> _users;
        #endregion

        #region Common 

        private readonly IMongoCollection<CommonModel> _common;

        #endregion
        
        #region Core
        private readonly IMongoClient _mongoClient = null;
        private readonly IMongoDatabase _context = null;
        private readonly IMongoCollection<Module> _module;
        private readonly IMongoCollection<Menu> _menu;
        
        private readonly IMongoCollection<Menu> _menuCongDan;
            
        private readonly IMongoCollection<DonVi> _donVi;

        
     
        private readonly IMongoCollection<UnitRole> _unitRole;
        
        
        private readonly IMongoCollection<FileModel> _file;
        


        private readonly Dictionary<string,  IMongoCollection<CommonModel>> _listCommonCollection;



        #endregion

        #region Nghiep vu

        private readonly IMongoCollection<DoanhNghiepModel> _doanhNghiep;
        private readonly IMongoCollection<HoaModel> _hoa;
        #endregion



        public DataContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.
               GetValue<string>(ConfigurationDb.MONGO_CONNECTION_STRING));
            if (client != null)
            {
                #region Core

                _context = client.GetDatabase(configuration.GetValue<string>(ConfigurationDb.MONGO_DATABASE_NAME));
                _users = _context.GetCollection<User>(DefaultNameCollection.USERS);
                _refreshToken = _context.GetCollection<RefreshTokenModel>(DefaultNameCollection.REFRESHTOKEN);                
                _menu = _context.GetCollection<Menu>(DefaultNameCollection.MENU);
                _menuCongDan = _context.GetCollection<Menu>(DefaultNameCollection.MENU_CONG_DAN);
                _module = _context.GetCollection<Module>(DefaultNameCollection.MODULE);
                _unitRole =_context.GetCollection<UnitRole>(DefaultNameCollection.UNIT_ROLE);
                _donVi = _context.GetCollection<DonVi>(DefaultNameCollection.DONVI);
                _file = _context.GetCollection<FileModel>(DefaultNameCollection.FILES);
                _listCommonCollection = new Dictionary<string,  IMongoCollection<CommonModel>>();
                foreach ( ItemCommon item in ListCommon.listCommon)
                {
                    IMongoCollection<CommonModel> colection = Database.GetCollection<CommonModel>(item.Code);
                    _listCommonCollection.Add(item.Code, colection);
                }

                #endregion

                #region NghiepVu
                _doanhNghiep = _context.GetCollection<DoanhNghiepModel>(DefaultNameCollection.DOANHNGHIEP);
                _hoa = _context.GetCollection<HoaModel>(DefaultNameCollection.HOA);

                #endregion

            }
        }


        #region Core

              
        public IMongoDatabase Database
        {
            get { return _context; }
        }
        public IMongoClient Client
        {
            get { return _mongoClient; }
        }

        public IMongoCollection<RefreshTokenModel> REFRESHTOKEN { get => _refreshToken; }
        
        
        public IMongoCollection<UnitRole> UNIT_ROLE { get => _unitRole; }

        
        public IMongoCollection<DonVi> DONVI { get => _donVi; }
       

        public IMongoCollection<User> USERS { get => _users; }
        
       
        
        
        
        public IMongoCollection<Module> MODULE { get => _module; }

        public IMongoCollection<Menu> MENU { get => _menu; }
        
        public IMongoCollection<Menu> MENUCONGDAN { get => _menuCongDan; }
        
        public IMongoCollection<FileModel> FILES { get => _file; }
        
        private Dictionary<string,  IMongoCollection<CommonModel>> CommonCollection { get => _listCommonCollection; }
        public  IMongoCollection<CommonModel> GetCategoryCollectionAs(string collectionName)
        {
            return  CommonCollection[collectionName];
        }

        #endregion

        #region NghiepVu

        public IMongoCollection<DoanhNghiepModel> DOANHNGHIEP { get => _doanhNghiep; }
        public IMongoCollection<HoaModel> HOA { get => _hoa; }

        #endregion
    }


}