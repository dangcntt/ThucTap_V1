using FestivalHoa.Properties.FromBodyModels;
using FestivalHoa.Properties.Models.PagingParam;

namespace FestivalHoa.Properties.Interfaces.Core
{
    public interface IDefaultReposityService<T> where T : class
    {
        Task<List<dynamic>> GetAll();
        Task<dynamic> GetById(IdFromBodyModel fromBodyModel);
        Task<bool> Delete(IdFromBodyModel fromBodyModel);
        
        Task<bool> Deleted(IdFromBodyModel fromBodyModel);
        Task<dynamic> GetPagingCore(PagingParamDefault pagingParam);
    }
}
