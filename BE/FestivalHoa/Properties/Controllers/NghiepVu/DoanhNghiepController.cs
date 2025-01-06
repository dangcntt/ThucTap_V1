using Microsoft.AspNetCore.Mvc;
using FestivalHoa.Properties.Constants;
using FestivalHoa.Properties.Installers;
using FestivalHoa.Properties.Interfaces.NghiepVu;
using FestivalHoa.Properties.Controllers.Core;
using FestivalHoa.Properties.Models.CongDan;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Models.PagingParam;

namespace FestivalHoa.Properties.Controllers.NghiepVu
{
    [Route("api/v1/[controller]")]
    public class DoanhNghiepController : DefaultReposityController<DoanhNghiepModel>
    {
        private readonly IDoanhNghiepService _service;
        private DataContext _dataContext;
        private static string NameCollection = DefaultNameCollection.DOANHNGHIEP;
        public DoanhNghiepController(DataContext context, IDoanhNghiepService service) : base(context, NameCollection)
        {
            _service = service;
            _dataContext = context;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] DoanhNghiepModel model)
        {
            try
            {

                var data = await _service.Create(model);
                return Ok(
                    new ResultMessageResponse()
                        .WithData(data)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.CREATE_SUCCESS)
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString).WithDetail(ex.Error)
                );
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] DoanhNghiepModel model)
        {
            try
            {

                var data = await _service.Update(model);
                return Ok(
                    new ResultMessageResponse()
                        .WithData(data)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.UPDATE_SUCCESS)
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString).WithDetail(ex.Error)
                );
            }
        }
    }
}
