using FestivalHoa.Properties.FromBodyModels;
using FestivalHoa.Properties.Models.CongDan;
using FestivalHoa.Properties.Models.PagingParam;

namespace FestivalHoa.Properties.Interfaces.NghiepVu
{

    public interface IHoaService
    {
        Task<dynamic> Create(HoaModel model);

        Task<dynamic> Update(HoaModel model);

        Task<dynamic> GetByCode(string code);
        
        
        Task<dynamic> GetPaging(PagingParam pagingParam );

        Task<dynamic> QRCode(string link);

        Task<dynamic> QRCode2();

        Task<dynamic> View();
    }
}

