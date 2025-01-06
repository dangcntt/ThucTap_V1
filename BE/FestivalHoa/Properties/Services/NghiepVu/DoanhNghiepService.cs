using FestivalHoa.Properties.Services.Core;
using FestivalHoa.Properties.Interfaces.NghiepVu;
using FestivalHoa.Properties.Extensions;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Installers;
using FestivalHoa.Properties.Models.CongDan;
using MongoDB.Driver;
using MongoDB.Bson;

namespace FestivalHoa.Properties.Services.NghiepVu
{
    public class DoanhNghiepService : BaseService, IDoanhNghiepService
    {
        private DataContext _context;
        private BaseMongoDb<DoanhNghiepModel, string> BaseMongoDb;

        public DoanhNghiepService(
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<DoanhNghiepModel, string>(_context.DOANHNGHIEP);

        }

        public async Task<dynamic> Create(DoanhNghiepModel model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

                var Doanhnghiep = new DoanhNghiepModel()
                {
                    Id = BsonObjectId.GenerateNewId().ToString(),
                    Name = model.Name,
                    DiaChi = model.DiaChi,
                    SDT = model.SDT,
                    Content = model.Content,
                    Link = model.Link
                    //QRCode = model.QRCode
                };
                //Doanhnghiep.CreatedBy = CurrentUser.UserName;
                ResultBaseMongo<DoanhNghiepModel> result;

                result = await BaseMongoDb.CreateAsync(Doanhnghiep);
                if (result.Entity.Id == default || !result.Success)
                    throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);

                return Doanhnghiep;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.ResultString).WithDetail(e.Error);
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

        public async Task<dynamic> Update(DoanhNghiepModel model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

                var Doanhnghiep = _context.DOANHNGHIEP.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();
                if (Doanhnghiep == default)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);

                Doanhnghiep.Name = model.Name;
                Doanhnghiep.DiaChi = model.DiaChi;
                Doanhnghiep.SDT = model.SDT;
                Doanhnghiep.Content = model.Content;
                Doanhnghiep.Link = model.Link;
                //Doanhnghiep.QRCode = model.QRCode;

                var result = await BaseMongoDb.UpdateAsync(Doanhnghiep);
                if (!result.Success)
                    throw new ResponseMessageException().WithException(DefaultCode.UPDATE_FAILURE);

                return Doanhnghiep;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.ResultString).WithDetail(e.Error);
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
    }
}