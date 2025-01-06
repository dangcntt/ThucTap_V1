using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using FestivalHoa.Properties.Constants;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.FromBodyModels;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Interfaces.Core;
using FestivalHoa.Properties.Models.PagingParam;

namespace FestivalHoa.Properties.Services.Core
{
    public class DefaultReposityService<T> : IDefaultReposityService<T> where T : class
    {
        protected IMongoCollection<T> _collection ;
    
        protected ProjectionDefinition<T, BsonDocument> projectionDefinition = Builders<T>.Projection
            .Exclude("CreatedAt")
            .Exclude("ModifiedAt")
            .Exclude("CreatedBy")
            .Exclude("ModifiedBy")
            .Exclude("IsDeleted")
            .Exclude("CreatedAtString")
            .Exclude("PasswordSalt")
            .Exclude("PasswordHash")
            .Exclude("UnsignedName");
        
        
        protected ProjectionDefinition<T, BsonDocument> projectionDefinitionGetAll = Builders<T>.Projection
            .Exclude("CreatedAt")
            .Exclude("ModifiedAt")
            .Exclude("CreatedBy")
            .Exclude("ModifiedBy")
            .Exclude("IsDeleted")
            .Exclude("CreatedAtString")
            .Exclude("PasswordSalt")
            .Exclude("PasswordHash")
            .Exclude("ListAction")
            .Exclude("ListMenu")
            .Exclude("Sort")
            .Exclude("UnsignedName");
        
        public DefaultReposityService(IMongoCollection<T> collection)
        {
            _collection = collection;
        }
        public async Task<List<dynamic>> GetAll()
        {
            try
          {
              var filter = Builders<T>.Filter.Eq("IsDeleted", false);
              var  listBson = await _collection.Find(filter).Project(projectionDefinitionGetAll).ToListAsync();
            if (listBson == null)
                return null;
            return  listBson.Select(x => BsonSerializer.Deserialize<dynamic>(x)).ToList();
          }
            catch (ResponseMessageException e) {
                throw new ResponseMessageException()
                    .WithCode(DefaultCode.EXCEPTION)
                    .WithMessage(e.ResultString);
            }
          catch (Exception ex)
          {
              if (ex.Message.Contains("is not a valid 24 digit hex string."))
              {
                  throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
              }
              throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(ex.Message);
          }
        }

        public async Task<dynamic> GetById(IdFromBodyModel fromBodyModel)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("IsDeleted", false);
                filter = Builders<T>.Filter.And(filter, Builders<T>.Filter.Eq("Id", fromBodyModel.Id));
                dynamic data = await _collection.Find(filter).FirstOrDefaultAsync();
                if (data == null)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);
                return data;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException()
                    .WithCode(DefaultCode.EXCEPTION)
                    .WithMessage(e.ResultString);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.Message);
            }
        }

        public async Task<bool> Delete(IdFromBodyModel fromBodyModel) 
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("IsDeleted", false);
                filter = Builders<T>.Filter.And(filter, Builders<T>.Filter.Eq("Id", fromBodyModel.Id));
                dynamic data =  await _collection.Find(filter).FirstOrDefaultAsync();
                if (data == null)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);
                data.IsDeleted = true;
                var result =  _collection.ReplaceOne(filter, data, new UpdateOptions() { IsUpsert = true } );
                if (result.ModifiedCount  ==   0  || result == null)
                    throw new ResponseMessageException().WithException(DefaultCode.DELETE_FAILURE);
                return true;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException()
                    .WithCode(DefaultCode.EXCEPTION)
                    .WithMessage(e.ResultString);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.Message);
            }
        }
        
        
        public async Task<bool> Deleted(IdFromBodyModel fromBodyModel) 
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("IsDeleted", false);
                filter = Builders<T>.Filter.And(filter, Builders<T>.Filter.Eq("Id", fromBodyModel.Id));
                dynamic data =  await _collection.Find(filter).FirstOrDefaultAsync();
                if (data == null)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);
            
            
                var result = await _collection.DeleteOneAsync(filter);
                if (!result.IsAcknowledged || result.DeletedCount <= 0)
                    throw new ResponseMessageException().WithException(DefaultCode.DELETE_FAILURE);
                return true;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException()
                    .WithCode(DefaultCode.EXCEPTION)
                    .WithMessage(e.ResultString);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.Message);
            }
        }
        

        public async Task<dynamic> GetPagingCore(PagingParamDefault pagingParam)
        {
            try
            {
                PagingModel<dynamic> result = new PagingModel<dynamic>();
                var builder = Builders<T>.Filter;
                var filter = builder.Empty;
                filter = builder.And(filter, builder.Eq("IsDeleted", false));
                if (!String.IsNullOrEmpty(pagingParam.Content))
                {
                    filter = builder.And(filter,
                        (builder.Regex("UnsignedName", FormatterString.ConvertToUnsign(pagingParam.Content)) |
                         builder.Regex("Code", pagingParam.Content) 
                        ));
                }
                
                if (pagingParam.PhanLoaiId != null && !pagingParam.PhanLoaiId.Equals(""))
                {
                    filter = builder.And(filter,
                        builder.Eq("PhanLoai._id", pagingParam.PhanLoaiId) 
                        );
                }
                
                if (pagingParam.DoanhNghiepId != null && !pagingParam.DoanhNghiepId.Equals(""))
                {
                    filter = builder.And(filter,
                        builder.Eq("DoanhNghiep._id", pagingParam.DoanhNghiepId) 
                    );
                }


                result.TotalRows = await _collection.CountDocumentsAsync(filter);

                string sortBy = pagingParam.SortBy != null ? FormatterString.HandlerSortBy(pagingParam.SortBy) : "CreatedAt";

                
                 var  list = await _collection.Find(filter)
                        .Sort(pagingParam.SortDesc
                            ? Builders<T>
                                .Sort.Descending(sortBy).Descending("CreatedAt")
                            : Builders<T>
                                .Sort.Ascending(sortBy).Descending("CreatedAt"))
                        .Skip(pagingParam.Skip)
                        .Limit(pagingParam.Limit)
                        .Project(projectionDefinition)
                        .ToListAsync();
                

            //   result.Data = list.Select(x => BsonSerializer.Deserialize<dynamic>(x)).ToList();
                result.Data =   list.Select(x => BsonSerializer.Deserialize<T>(x)).ToList();

                
                return result;
            }
            catch (ResponseMessageException e)
            {
                new ResultMessageResponse().WithCode(e.ResultCode)
                    .WithMessage(e.ResultString);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }

                if (e.Message.Contains("ObjectSerializer does not support BSON type 'Binary'."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.SORT_BY_NOT_EXIST);
                }
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.Message);
            }
            return null;
        }
    }
    
    
}
