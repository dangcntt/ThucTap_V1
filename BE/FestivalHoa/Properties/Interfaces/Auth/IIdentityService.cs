using FestivalHoa.Properties.Models.Auth;
using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.Interfaces.Auth
{
    public interface IIdentityService
    {
        Task<User> Authenticate(AuthRequest model);
        Task<dynamic> LoginAsync(AuthRequest model);
   
    }
}