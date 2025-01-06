using MongoDB.Bson;
using MongoDB.Driver;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.Extensions;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Installers;
using FestivalHoa.Properties.Interfaces.Core;
using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.Services.Core
{
    public class UnitRoleService : BaseService, IUnitRoleService
    {
        private DataContext _context;
        private BaseMongoDb<UnitRole, string> BaseMongoDb;
        private BaseMongoDb<User, string> BaseMongoDbUser;
        private IMenuService _menuService;

        public UnitRoleService( DataContext context,
            IHttpContextAccessor contextAccessor,
            IMenuService menuService
        )
            : base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<UnitRole, string>(_context.UNIT_ROLE);
        }

        public async Task<dynamic> Create(UnitRole model)
        {
            try
            {
                if (model == default) throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

                var checkName = _context.UNIT_ROLE.Find(x => x.Name == model.Name && !x.IsDeleted).FirstOrDefault();

                if (checkName != default) throw new ResponseMessageException().WithException(DefaultCode.DATA_EXISTED);

             

                var entity = new UnitRole
                {
                    Id = BsonObjectId.GenerateNewId().ToString(),
                    Name = model.Name,
                    Code = model.Code,
                    Sort = model.Sort,
                    CreatedAt = DateTime.Now,
                    CreatedBy = CurrentUserName,
                };
                /*entity.SetUnitRole(_donViService);*/
                var result = await BaseMongoDb.CreateAsync(entity);
                if (result.Entity.Id == default || !result.Success)
                    throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
                return new UnitRoleShortShow(entity);
            }
            catch (ResponseMessageException e)
            {
                new ResultMessageResponse()
                    .WithCode(DefaultCode.EXCEPTION)
                    .WithMessage(e.ResultString)
                    .WithDetail(e.Error);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(ex.Message);
            }

            return null;
        }
        public async Task<dynamic> Update(UnitRole model)
        {
            try
            {
                if (model == default)  throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

            var entity = _context.UNIT_ROLE.Find(x => x.Id == model.Id).FirstOrDefault();
            if (entity == default) throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

            
            var checkName = _context.UNIT_ROLE.Find(x => x.Id != model.Id
                                                            && x.Name.ToLower() == model.Name.ToLower()
                                                            && !x.IsDeleted 
            ).FirstOrDefault();
            
            if (checkName != default) throw new ResponseMessageException().WithException(DefaultCode.DATA_EXISTED);

            

            entity.Name = model.Name;
            entity.Code = model.Code;
            entity.Sort = model.Sort;
            entity.ModifiedAt = DateTime.Now;
            entity.CreatedBy = CurrentUserName;
            /*entity.SetUnitRole(_donViService);*/
            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
                throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);
            
            
            await UpdateRolesInUser(entity);
            return new UnitRoleShortShow(entity);;
            }
            catch (ResponseMessageException e)
            {
                new ResultMessageResponse().WithCode(DefaultCode.EXCEPTION)
                    .WithMessage(e.ResultString)
                    .WithDetail(e.Error);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("is not a valid 24 digit hex string."))
                {
                    throw new ResponseMessageException().WithException(DefaultCode.ID_NOT_CORRECT_FORMAT);
                }
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(ex.Message);
            }

            return null;
        }
        
        
        
        public async Task<dynamic> UpdateAction(UnitRole model)
        {
            try
            {
                if (model == default) throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var entity = _context.UNIT_ROLE.Find(x => x.Id == model.Id).FirstOrDefault();
                if (entity == default) throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);
                if (model.ListAction != null)
                {
                    entity.SetAction(model.ListAction);
                }
                entity.ModifiedAt = DateTime.Now;
                var result = await BaseMongoDb.UpdateAsync(entity);
                if (!result.Success) throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);
                return entity.ListAction;
            } 
        catch (ResponseMessageException e)
        {
            new ResultMessageResponse().WithCode(e.ResultCode)
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

            return null;

        }

        private async Task UpdateRolesInUser(UnitRole role)
        {
            var filterUser = Builders<User>.Filter.Where(x => x.UnitRole != null && x.UnitRole.Id ==  role.Id);
            var update = Builders<User>.Update
                .Set(s => s.UnitRole, new UnitRoleShort(role))
                .Set(s => s.ModifiedBy, CurrentUser.UserName)
                .Set(s => s.ModifiedAt, DateTime.Now);
            UpdateResult actionResult
                = await _context.USERS.UpdateManyAsync(filterUser, update);
        }
        
    }
    
}