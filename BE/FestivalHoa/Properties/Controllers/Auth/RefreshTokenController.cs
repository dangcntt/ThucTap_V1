using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Interfaces.Auth;
using FestivalHoa.Properties.Models.Auth;

namespace FestivalHoa.Properties.Controllers.Auth
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IRefreshTokenService _service;
        public RefreshTokenController(IRefreshTokenService service)
        {
            _service = service;
        }
        
        [HttpPost]
        [Route("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenApiModel model)
        {
            try
            {
                var response = await _service.RefreshToken(model);
                
                return Ok(
                    new ResultMessageResponse()
                        .WithData(response)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage("Refresh Token thành công !")
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