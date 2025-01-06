using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using FestivalHoa.Properties.Models.Auth;

namespace FestivalHoa.Properties.Interfaces.Auth
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenModel> Create(RefreshTokenModel model);
        
        
        
        
        Task<SecurityToken> CreateJwtSecurityToken(JwtSecurityTokenHandler tokenHandler  , string userName , string id);
        
        
        Task<dynamic> RefreshToken(TokenApiModel model);


        public  string GenerateRefreshToken(); 

    }
}