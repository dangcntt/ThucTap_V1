using FestivalHoa.Properties.Models.CongDan;

namespace FestivalHoa.Properties.Interfaces.NghiepVu
{

    public interface IDoanhNghiepService
    {
        Task<dynamic> Create(DoanhNghiepModel model);

        Task<dynamic> Update(DoanhNghiepModel model);

    }
}

