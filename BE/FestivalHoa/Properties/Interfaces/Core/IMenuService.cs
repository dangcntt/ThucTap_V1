using FestivalHoa.Properties.Models.Core;
using FestivalHoa.Properties.ViewModels;

namespace FestivalHoa.Properties.Interfaces.Core
{
    public interface IMenuService
    {
        Task<List<MenuTreeVM>> GetTreeList();
        
        
        Task<List<MenuTreeAndActionVM>> GetTreeListAndAction();
        
        Task<dynamic> GetTreeFlatten();
        
        
        
        
        Task<dynamic> Create(Menu model);
        Task<dynamic> Update(Menu model);
        Task<dynamic> AddAC(MenuList model);
        Task<dynamic>  DeleteAc(MenuList model);
        
        
        Task<List<NavMenuVM>> GetTreeListMenuForCurrentUser(List<Menu> listMenu);


        Task<List<NavMenuVM>> GetTreeListMenuAll(); 
    }
}