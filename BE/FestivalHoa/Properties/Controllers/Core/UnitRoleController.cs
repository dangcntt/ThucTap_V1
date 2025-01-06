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
   //  [Authorize]
    public class UnitRoleController : DefaultReposityController<UnitRole>
    {
    private readonly IUnitRoleService _service;
    private DataContext _dataContext;
    private static string NameCollection = DefaultNameCollection.UNIT_ROLE;

        public UnitRoleController(DataContext context, IUnitRoleService service) : base(context, NameCollection)
        {
            _service = service;
        }
        
        
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] UnitRole model)
        {
            try
            {
                var response = await _service.Create(model);
                return Ok(
                    new ResultMessageResponse()
                        .WithData(response)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.CREATE_SUCCESS)
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UnitRole model)
        {
            try
            {
                var response = await _service.Update(model);

                return Ok(
                    new ResultMessageResponse()
                        .WithData(response)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.UPDATE_SUCCESS)
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }
        }
        
        
        [HttpPost]
        [Route("update-action")]
        public async Task<IActionResult> UpdateAction([FromBody] UnitRole model)
        {
            try
            {
                var response = await _service.UpdateAction(model);

                return Ok(
                    new ResultMessageResponse()
                        .WithData(response)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.UPDATE_SUCCESS)
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }
        }
        
    }
}