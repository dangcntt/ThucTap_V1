using FestivalHoa.Properties.Models.Core;
using FestivalHoa.Properties.ViewModels;

namespace FestivalHoa.Properties.Interfaces.Core;

public interface IUserService
{
    
    Task<User> Create(User model);
    Task<User> Update(User model);
    
    Task<User> GetByUserName(string userName);  
    Task<User> ChangePassword(UserVM model);
    Task<User> ChangeProfile(User model);

}