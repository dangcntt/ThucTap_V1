using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using FestivalHoa.Properties.Constants;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.Extensions;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Installers;
using FestivalHoa.Properties.Interfaces.Auth;
using FestivalHoa.Properties.Interfaces.Core;
using FestivalHoa.Properties.Models.Auth;
using FestivalHoa.Properties.Models.Core;
using FestivalHoa.Properties.Models.Settings;
using FestivalHoa.Properties.Services.Core;

namespace FestivalHoa.Properties.Services.Auth
{
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IMenuService _menuService;
        private DataContext _context;

        public IdentityService(
            DataContext context,
            IUserService userService,
            IRefreshTokenService refreshTokenService,
            JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters,
            IHttpContextAccessor httpContext,
            IMenuService menuService
        ) : base(context, httpContext)
        {
            _context = context;
            _userService = userService;
            _refreshTokenService = refreshTokenService;
            _jwtSettings = jwtSettings;
            _menuService = menuService;
            _tokenValidationParameters = tokenValidationParameters;
        }
        
        

        public async Task<dynamic> LoginAsync(AuthRequest model)
        {
            var user = await Authenticate(model);
            if (!user.IsActived)
            {
                throw new ResponseMessageException().WithException(DefaultCode.ACCOUNT_IS_LOCKED);
            }
            return await GenerateAuthenticationResultForUserAsync(user);
        }


        public async Task<User> Authenticate(AuthRequest model)
        {
            var docs = _context.USERS.Aggregate()
                .Match(Builders<User>.Filter.Where(x=>!x.IsDeleted  && x.UserName == model.Username ))
                .Lookup(DefaultNameCollection.UNIT_ROLE,
                    "UnitRole._id",
                    "_id",
                    "ListUnitRole")
                .As<BsonDocument>()
                .FirstOrDefault();
            if (docs == null)
            {
                throw new ResponseMessageException().WithException(DefaultCode.USERNAME_NOT_FOUND);
            }
           if (docs["ListUnitRole"] == null   || docs["ListUnitRole"].AsBsonArray.Count == 0  )
            {
                throw new ResponseMessageException().WithCode(DefaultCode.ACCOUNT_NOT_AUTHORIZED)
                    .WithMessage( $"Tài khoản {model.Username} chưa được thiết lập quyền !");
            }
            var role= BsonSerializer.Deserialize<UnitRole>(docs["ListUnitRole"][0].ToBsonDocument());
                if (role == null || role.ListMenu.Count == 0 || role.ListMenu == default) 
                {
                    throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION)
                        .WithMessage($"Tài khoản {model.Username} chưa được thiết lập menu !");
                }
                docs.ToBsonDocument().Remove("ListUnitRole");
                
                var user = BsonSerializer.Deserialize<User>(docs);
         
                if (!PasswordExtensions.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.WRONG_PASSWORD);
                }
            if (PasswordExtensions.VerifyPasswordHash("DongThap@123", user.PasswordHash, user.PasswordSalt))
            {
                user.IsRequireChangePassword = true;
            }
            user.UnitRole= new UnitRoleShort(role);
            
            if (role.ListMenu.Count > 0)
            {
                List<Menu> listMenus = await _context.MENU
                    .Find(x => role.ListMenu.Contains(x.UnsignedName) && !x.IsDeleted).ToListAsync();
                if (listMenus.Count > 0)
                {
                    var menu = await _menuService.GetTreeListMenuAll();
                    user.Menu = menu;
                }
            }
            return user;
        }
        
        private async Task<dynamic> GenerateAuthenticationResultForUserAsync(User user)
        {

            if (user == null) throw new ResponseMessageException().WithException(DefaultCode.EXCEPTION);

            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await _refreshTokenService.CreateJwtSecurityToken(tokenHandler,user.UserName,user.Id);
            
            var refreshToken = new RefreshTokenModel
            {
                JwtId = token.Id,
                UserId = user.Id,
                AccessToken = tokenHandler.WriteToken(token),
                CreationDateToken = DateTime.UtcNow,
                ExpiryDateToken = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                ExpiryDateRefreshToken = FormatTime.ConvertToUnixTimestamp(DateTime.UtcNow.Add(_jwtSettings.TokenRefreshStore)),
                RefreshToken =  _refreshTokenService.GenerateRefreshToken()
            };
            var createdRefreshToken = await _refreshTokenService.Create(refreshToken);
            var userLogin = new UserLogin(user,refreshToken.ExpiryDateToken);
            userLogin.AccessToken = refreshToken.AccessToken;
            userLogin.RefreshToken = refreshToken.RefreshToken;
            return userLogin;
        }

        
   

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        
        
    }
}