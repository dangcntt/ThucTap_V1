using Microsoft.AspNetCore.Mvc;
using FestivalHoa.Properties.Constants;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Installers;
using FestivalHoa.Properties.Interfaces.Core;
using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.Controllers.Core
{
    [Route("api/v1/[controller]")]
 //   [Authorize]
    public class MenuCongDanController : DefaultReposityController<Menu>
    {
        private readonly IMenuCongDanService _service;
        private DataContext _dataContext;
        private static string NameCollection = DefaultNameCollection.MENU_CONG_DAN;
        public MenuCongDanController(DataContext context, IMenuCongDanService service) : base(context, NameCollection)
        {
            _service = service;
            _dataContext = context;
        }
        
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] Menu model)
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
        public async Task<IActionResult> Update([FromBody] Menu model)
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
      

        [HttpPost]
        [Route("add-action")]
        public async Task<IActionResult> AddAC([FromBody] MenuList model)
        {
            try
            {

                var data = await _service.AddAC(model);
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
        [Route("delete-action")]
        public async Task<IActionResult> DeleteAc([FromBody] MenuListShort model)
        {
            try
            {
                var data = await _service.DeleteAc(model); 

                return Ok(
                    new ResultMessageResponse()
                        .WithData(data)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.DELETE_SUCCESS)
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






        [HttpGet]
        [Route("getTreeList")]
        public async Task<IActionResult> GetTreeList()
        {
            try
            {
                var response = await _service.GetTreeList();
                return Ok(new ResultMessageResponse()
                    .WithData(response)
                    .WithCode(DefaultCode.SUCCESS)
                    .WithMessage(DefaultMessage.GET_DATA_SUCCESS));
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString).WithDetail(ex.Error)
                );
            }
            
        }
        
        
             
        [HttpGet]
        [Route("getTreeFlatten")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _service.GetTreeFlatten();
                return Ok(
                    new ResultMessageResponse()
                        .WithData(response)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
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