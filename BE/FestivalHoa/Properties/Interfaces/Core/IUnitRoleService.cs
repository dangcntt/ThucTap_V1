using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.Interfaces.Core
{
    public interface IUnitRoleService
    {
        Task<dynamic> Create(UnitRole model);
        Task<dynamic> Update(UnitRole model);
        
        
        Task<dynamic> UpdateAction(UnitRole model);
        




    }
}