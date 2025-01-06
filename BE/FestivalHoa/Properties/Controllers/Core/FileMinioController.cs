using Microsoft.AspNetCore.Mvc;
using Minio;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Interfaces.Core;

namespace FestivalHoa.Properties.Controllers.Core
{
    [Route("api/[controller]")]
    public class FilesMinioController : Controller
    {

        private readonly IMinioClient _minio;
        private readonly IFileMinioService _fileMinioService;
        public FilesMinioController(
            IMinioClient minio,
            IFileMinioService fileMinioService
        )
        {
            _minio = minio;
            _fileMinioService = fileMinioService;
        }


        [HttpPost]
        [Route("~/api/v1/filesminio/upload")]
        public async Task<IActionResult> Update(IFormFile files)
        {
            try
            {

                var file = await _fileMinioService.UploadFile(files);

                return Ok(
                    new ResultMessageResponse()
                        .WithData(file)
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
        [Route("~/api/v1/filesminio/delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {

                var key = await _fileMinioService.DeleteFile(id);

                return Ok(
                    new ResultMessageResponse()
                        .WithData(key)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.DELETE_SUCCESS)
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
        [Route("~/api/v1/filesminio/dowload/{id}")]
        public async Task<IActionResult> Dowload(string id)
        {
            try
            {

                var key = await _fileMinioService.Dowload(id);

                return Ok(
                    new ResultMessageResponse()
                        .WithData(key)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.DOWLOAD_SUCCESS)
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
        [Route("~/api/v1/filesminio/uploadCK")]
        public async Task<IActionResult> UpdateCK(IFormFile upload)
        {
            try
            {
                var file = await _fileMinioService.UploadFileCK(upload);

                return Ok(
                    new {url=file}
                    
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




        [HttpGet]
        [Route("~/api/v1/filesminio/view/{id}")]
        public async Task<IActionResult> ViewFile(string id)
        {

            try
            {
                var data = await _fileMinioService.GetById(id);
                if (data == null)
                {
                    return Ok(new ResultMessageResponse().WithCode(DefaultCode.DATA_NOT_FOUND)
                        .WithMessage(DefaultMessage.DATA_NOT_FOUND));
                }
                MemoryStream memory = new MemoryStream();
                var memorystream = await _fileMinioService.Dowload(id);
                if (memorystream != null)
                {
                    return File(memorystream, "application/octet-stream", Path.GetFileName(data.FileName));
                }
                return Ok(new ResultMessageResponse().WithCode(DefaultCode.ERROR_STRUCTURE)
                        .WithMessage(DefaultMessage.ERROR_STRUCTURE + data.FileName + " | " + memorystream));
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