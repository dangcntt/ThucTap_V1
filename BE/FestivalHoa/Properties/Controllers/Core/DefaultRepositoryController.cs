using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.FromBodyModels;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Installers;
using FestivalHoa.Properties.Interfaces.Core;
using FestivalHoa.Properties.Models.PagingParam;
using FestivalHoa.Properties.Services.Core;

namespace FestivalHoa.Properties.Controllers.Core
{
    public abstract class DefaultReposityController<T> : ControllerBase where T : class
    {
        protected readonly IDefaultReposityService<T> Repository;
        public IMongoCollection<T> _collection;
        
        public DefaultReposityController(DataContext context ,  string collectionName)
        {
            _collection = context.Database.GetCollection<T>(collectionName);
            Repository = new DefaultReposityService<T>(_collection);
         //   Repository.getCollection(_collection);
        }
       
        
        [HttpGet]
        [Route("get-all-core")]
        public virtual async Task<IActionResult> GetAllData()
        {
            try
            {
                var response = await  Repository.GetAll();
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
                        .WithMessage(ex.ResultString)
                );
            }
        }
        
        
        [HttpPost]
        [Route("get-by-id-core")]
        public async Task<IActionResult> GetById([FromBody] IdFromBodyModel model)
        {
            try
            {
                var response = await  Repository.GetById(model);
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
                        .WithMessage(ex.ResultString)
                );
            }
        }
        
        
        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] IdFromBodyModel model)
        {
            try
            {
                await  Repository.Delete(model);
                return Ok(
                    new ResultMessageResponse()
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
        [Route("deleted")]
        public async Task<IActionResult> Deleted([FromBody] IdFromBodyModel model)
        {
            try
            {
                await  Repository.Deleted(model);
                return Ok(
                    new ResultMessageResponse()
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
        [Route("get-paging-params-core")]
        public virtual async Task<IActionResult> GetPagingCore([FromBody] PagingParamDefault pagingParam)
        {
            try
            {
                var response = await  Repository.GetPagingCore(pagingParam);
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
                        .WithMessage(ex.ResultString)
                );
            }
        }


    }
}