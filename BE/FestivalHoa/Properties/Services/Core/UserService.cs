using MongoDB.Driver;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.Extensions;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Installers;
using FestivalHoa.Properties.Interfaces.Core;
using FestivalHoa.Properties.Models.Core;
using FestivalHoa.Properties.ViewModels;

namespace FestivalHoa.Properties.Services.Core
{
    public class UserService : BaseService, IUserService
    {
        private DataContext _context;
        private BaseMongoDb<User, string> BaseMongoDb;
        IMongoCollection<User> _collectionUser;
        public UserService(
            DataContext context, 
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<User, string>(_context.USERS);
            _collectionUser = context.USERS;
        }
        
        
        
        public async Task<User> GetByUserName(string userName)
        {
            return await _context.USERS.Find(x => x.UserName == userName && x.IsDeleted != true).FirstOrDefaultAsync();
        }

        public async Task<User> Create(User model)
        {
            if (model == default)
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
            var checkName = _context.USERS.Find(x => x.UserName == model.UserName && x.IsDeleted != true)
                .FirstOrDefault();

            if (checkName != default)
            {
                throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);
            }

            var entity = new User
            {
                UserName = model.UserName,
                Name = model.UserName,
                FullName = model.FullName,
                Phone = model.Phone,
                Email = model.Email,
                UnitRole = model.UnitRole,
                IsVerified = model.IsVerified,
                IsSyncPasswordSuccess = model.IsSyncPasswordSuccess,
                IsActived = model.IsActived,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };


            byte[] passwordHash, passwordSalt;
            if (string.IsNullOrEmpty(model.Password))
            {
                model.Password = "DongThap@123";
            }
            PasswordExtensions.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
            entity.PasswordHash = passwordHash;
            entity.PasswordSalt = passwordSalt;

            var result = await BaseMongoDb.CreateAsync(entity);
            if (result.Entity.Id == default || !result.Success)
            {
                throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
            }
            return entity;
        }

        public async Task<User> Update(User model)
        {
            if (model == default)
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
            var entity = _context.USERS.Find(x => x.Id == model.Id).FirstOrDefault();

            if (entity == default)
                throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

            var checkName = _context.USERS
                .Find(x => x.Id != model.Id && x.UserName == model.UserName && x.IsDeleted != true).FirstOrDefault();

            if (checkName != default)
                throw new ResponseMessageException().WithCode(DefaultCode.DATA_EXISTED)
                    .WithMessage("Tên tài khoản đã tồn tại");

            entity.UserName = model.UserName;
            entity.FullName = model.FullName;
            entity.Phone = model.Phone;
            entity.Email = model.Email;
            entity.UnitRole = model.UnitRole;
            entity.ModifiedAt = DateTime.Now;
            entity.IsVerified = model.IsVerified;
            entity.IsSyncPasswordSuccess = model.IsSyncPasswordSuccess;
            entity.IsActived = model.IsActived;
            if (!string.IsNullOrEmpty(model.Password))
            {
                byte[] passwordHash, passwordSalt;
                PasswordExtensions.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
                entity.PasswordHash = passwordHash;
                entity.PasswordSalt = passwordSalt;
            }

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);
            }
            
            return entity;
        }

        public async Task<User> ChangeProfile(User model)
        {
            if (model == default)
            {
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
            }

            var entity = _context.USERS.Find(x => x.Id == model.Id && !x.IsDeleted).FirstOrDefault();
            if (entity == default)
            {
                throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);
            }


            entity.FullName = model.FullName;
            entity.Phone = model.Phone;
            entity.Email = model.Email;
            entity.Avatar = model.Avatar;

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);
            }
            
            return entity;
        }

        public async Task<User> ChangePassword(UserVM model)
        {
            if (model == default)
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
            var entity = _context.USERS.Find(x => x.UserName == model.UserName && !x.IsDeleted).FirstOrDefault();
            if (entity == default)
                throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);
            if (!string.IsNullOrEmpty(model.Password))
            {
                byte[] passHash, passSalt;
                passHash = entity.PasswordHash;
                passSalt = entity.PasswordSalt;
                var pass = PasswordExtensions.VerifyPasswordHash(model.Password, passHash, passSalt);
                if (pass != true)
                {
                    throw new ResponseMessageException().WithCode(DefaultCode.UPDATE_FAILURE)
                        .WithMessage("Mật khẩu không chính xác");
                }
                else
                {
                    if (model.newPass == model.confirmPass)
                    {
                        byte[] passwordHash, passwordSalt;
                        PasswordExtensions.CreatePasswordHash(model.newPass, out passwordHash, out passwordSalt);
                        entity.PasswordHash = passwordHash;
                        entity.PasswordSalt = passwordSalt;
                    }
                }
            }

            var result = await BaseMongoDb.UpdateAsync(entity);
            if (!result.Success)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.UPDATE_FAILURE)
                    .WithMessage("Đổi mật khẩu thất bại.");
            }
            
            return entity;
        }


    }
}
