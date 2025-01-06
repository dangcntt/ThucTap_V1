using FestivalHoa.Properties.Models.Core;
using FestivalHoa.Properties.ViewModels;

namespace FestivalHoa.Properties.Interfaces.Core
{
    public interface IMenuCongDanService
    {
        Task<List<MenuTreeVM>> GetTreeList();
        
        Task<dynamic> GetTreeFlatten();
        
        
        Task<dynamic> Create(Menu model);
        Task<dynamic> Update(Menu model);
        Task<dynamic> AddAC(MenuList model);
        Task<dynamic>  DeleteAc(MenuListShort model);
        
        
        Task<List<NavMenuVM>> GetTreeListMenuForCurrentUser(List<Menu> listMenu);


        Task<List<NavMenuVM>> GetTreeListMenuAll(); 
    }
}