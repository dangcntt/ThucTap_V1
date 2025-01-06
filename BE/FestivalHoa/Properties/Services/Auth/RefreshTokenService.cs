using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using FestivalHoa.Properties.Constants;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.Extensions;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Installers;
using FestivalHoa.Properties.Interfaces.Auth;
using FestivalHoa.Properties.Models.Auth;
using FestivalHoa.Properties.Models.Settings;
using FestivalHoa.Properties.Services.Core;

namespace FestivalHoa.Properties.Services.Auth
{
    public class RefreshTokenService : BaseService, IRefreshTokenService
    {
        private DataContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly JwtSettings _jwtSettings;
        private BaseMongoDb<RefreshTokenModel, string> BaseMongoDb;
        private HttpContext _httpContext { get { return _contextAccessor.HttpContext; } }
        public RefreshTokenService(
            DataContext context,
            TokenValidationParameters tokenValidationParameters,
            JwtSettings jwtSettings,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            BaseMongoDb = new BaseMongoDb<RefreshTokenModel, string>(_context.REFRESHTOKEN);
        }

        public async Task<RefreshTokenModel> Create(RefreshTokenModel model)
        {
            if (model == default)
            {
                throw new ResponseMessageException().WithException(DefaultCode.EXCEPTION);
            }
            var result = await BaseMongoDb.CreateAsync(model);
                if (result.Entity.Id == default || !result.Success)
                {
                    throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
                }
                return model;
        }

        public async Task<SecurityToken> CreateJwtSecurityToken(JwtSecurityTokenHandler tokenHandler ,string userName , string id)
        {
            
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userName),
                new Claim(ListActionDefault.KeyId, id)
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token; 
        }


        public async Task<dynamic> RefreshToken(TokenApiModel model)
        {
            if (model == default)
                throw new ResponseMessageException().WithException(DefaultCode.EXCEPTION);
            
            var resultRefreshToken = ValidateJwtToken(model.AccessToken);
            
            if (resultRefreshToken.Username == null || resultRefreshToken.UserId == null)
                throw new ResponseMessageException().WithException(DefaultCode.TOKEN_NOT_FOUND);


            var refreshToken = _context.REFRESHTOKEN.Find(x => !x.IsDeleted
                                                               && !x.Invalidated &&
                                                               x.RefreshToken == model.RefreshToken &&
                                                               x.AccessToken == model.AccessToken && 
                                                               x.UserId == resultRefreshToken.UserId).FirstOrDefault();
            
            if (refreshToken == null)
                throw new ResponseMessageException().WithException(DefaultCode.TOKEN_OR_REFRESH_TOKEN_NOT_FOUND);

            var today = FormatTime.ConvertToUnixTimestamp(DateTime.UtcNow);

            if (refreshToken.ExpiryDateRefreshToken - today < 0)
            {
                throw new ResponseMessageException().WithException(DefaultCode.REFRESH_TOKEN_OUT_TIME);
            }
            
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await CreateJwtSecurityToken(tokenHandler, resultRefreshToken.Username,resultRefreshToken.UserId);

            if (token == null || token.Equals(""))
                throw new ResponseMessageException().WithException(DefaultCode.EXCEPTION);

            
            var refreshTokenModel = new RefreshTokenModel
            {
                JwtId = token.Id,
                UserId = resultRefreshToken.UserId,
                AccessToken = tokenHandler.WriteToken(token),
                CreationDateToken = DateTime.UtcNow,
                ExpiryDateToken = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                ExpiryDateRefreshToken = FormatTime.ConvertToUnixTimestamp(DateTime.UtcNow.Add(_jwtSettings.TokenRefreshStore)),
                RefreshToken =  GenerateRefreshToken()
            };

            if (refreshTokenModel.AccessToken == null || refreshTokenModel.RefreshToken == null)
                throw new ResponseMessageException().WithException(DefaultCode.EXCEPTION);
            var createdRefreshToken = Create(refreshTokenModel);
            
            return new RefreshTokenResultModel(refreshTokenModel.AccessToken , refreshTokenModel.RefreshToken , refreshToken.ExpiryDateToken );

        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        
        public ResultRefreshToken? ValidateJwtToken(string token)
        {
            if (token == null || token.Equals(""))
                return null;
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;
                var result = new ResultRefreshToken()
                {
                    Username = tokenS.Claims.First(claim => claim.Type == ListActionDefault.KeyId).Value,
                    UserId = tokenS.Claims.First(claim => claim.Type == ListActionDefault.KeyId).Value,
                };
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}