using MongoDB.Driver;
using FestivalHoa.Properties.Constants;
using FestivalHoa.Properties.Installers;
using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.Services.Core
{
    public abstract class BaseService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DataContext _context;
        private User _appUser;
        
        
        protected User CurrentUser => GetCurrentUser();
        
        protected string CurrentUserName => GetCurrentUser()?.UserName;
        private HttpContext _httpContext { get { return _contextAccessor.HttpContext; } }
        
        

        public BaseService()
        {
        }
        public BaseService(DataContext context,
            IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }


        private User GetCurrentUser()
        {
            if (_appUser != null)
                return _appUser;

            var a = _httpContext.User?.Claims; 
            var userId = _httpContext.User?.Claims.FirstOrDefault(x => x.Type == ListActionDefault.KeyId)?.Value;
            if (userId != default)
                _appUser = _context.USERS.Find(x => x.Id == userId).FirstOrDefault();
            return _appUser;
        }

        
        
    }
}