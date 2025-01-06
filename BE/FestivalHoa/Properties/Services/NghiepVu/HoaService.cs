using FestivalHoa.Properties.Services.Core;
using FestivalHoa.Properties.Interfaces.NghiepVu;
using FestivalHoa.Properties.Extensions;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Installers;
using FestivalHoa.Properties.Models.CongDan;
using MongoDB.Driver;
using MongoDB.Bson;
using FestivalHoa.Properties.Interfaces.Core;
using ZXing;
using System.Drawing;
using FestivalHoa.Properties.Constants;
using FestivalHoa.Properties.Models.PagingParam;
using MongoDB.Bson.Serialization;
using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.Services.NghiepVu
{
    public class HoaService : BaseService, IHoaService
    {
        private DataContext _context;
        private BaseMongoDb<HoaModel, string> BaseMongoDb;
        private readonly IFileMinioService _fileMinioService;
        public HoaService(
            DataContext context,
            IHttpContextAccessor contextAccessor,
            IFileMinioService fileMinioService) :
            base(context, contextAccessor)
        {
            _context = context;
            BaseMongoDb = new BaseMongoDb<HoaModel, string>(_context.HOA);
            _fileMinioService = fileMinioService;
        }

        public async Task<dynamic> Create(HoaModel model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var Hoa = new HoaModel()
                {
                    Id = BsonObjectId.GenerateNewId().ToString(),
                    Name = model.Name,
                    Avatar = model.Avatar,
                    PhanLoai = model.PhanLoai,
                    Files = model.Files,
                    NoiDung = model.NoiDung,
                    MoTa = model.MoTa,
                    QuaTrinhCanhTac = model.QuaTrinhCanhTac,
                    DoanhNghiep = model.DoanhNghiep,
                    Sort = model.Sort,
                    EngName = model.EngName,
                    CayGiong = model.CayGiong,
                    SauBenh = model.SauBenh,
                    GiaThe = model.GiaThe,
                    TrongChau = model.TrongChau,
                    TuoiNuoc = model.TuoiNuoc,
                    BonPhan = model.BonPhan,
                    CatTia = model.CatTia,
                    ChamSoc = model.ChamSoc,
                };
                var countcode = _context.HOA.Find(x => !x.IsDeleted && !string.IsNullOrEmpty(x.Code)).SortByDescending(x => x.Id).FirstOrDefault();
                int intValue = int.Parse(countcode.Code) + 1;
                Hoa.Code = "0" + intValue.ToString();

                //Hoa.CreatedBy = CurrentUser.UserName;
                int width = 1000;
                int height = 1000;
                BarcodeWriter barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                barcodeWriter.Options = new ZXing.Common.EncodingOptions
                {
                    Width = width,
                    Height = height
                };

                //Bitmap qrCodeBitmap = barcodeWriter.Write("https://festivalhoa.dongthap.gov.vn/hoa/chi-tiet/" + Hoa.Code);
                Bitmap qrCode2Bitmap = barcodeWriter.Write("https://hoasadec.com.vn/hoa/chi-tiet/" + Hoa.Code);
                using (MemoryStream stream = new MemoryStream())
                {
                    qrCode2Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    var fileshort = _fileMinioService.UploadImg(stream, Hoa.Name);
                    Hoa.QRCode2 = fileshort.Result;
                }
                ResultBaseMongo<HoaModel> result;

                result = await BaseMongoDb.CreateAsync(Hoa);
                if (result.Entity.Id == default || !result.Success)
                    throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);

                return Hoa;
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

        public async Task<dynamic> Update(HoaModel model)
        {
            try
            {
                 if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

                var Hoa = _context.HOA.Find(x => !x.IsDeleted && x.Id == model.Id).FirstOrDefault();
                if (Hoa == default)
                    throw new ResponseMessageException().WithException(DefaultCode.DATA_NOT_FOUND);


                var filter = Builders<HoaModel>.Filter.Where(x => x.Id == model.Id);
            var update = Builders<HoaModel>.Update
                .Set(x => x.Name, model.Name)
                .Set(x => x.Avatar, model.Avatar)
                .Set(x => x.PhanLoai, model.PhanLoai)
                .Set(x => x.NoiDung, model.NoiDung)
                .Set(x => x.QuaTrinhCanhTac, model.QuaTrinhCanhTac)
                .Set(x => x.DoanhNghiep, model.DoanhNghiep)
                .Set(x => x.MoTa, model.MoTa)
                .Set(x => x.Sort, model.Sort)
                .Set(x => x.Files, model.Files)
                .Set(x => x.EngName, model.EngName)
                .Set(x => x.CayGiong, model.CayGiong)
                .Set(x => x.SauBenh, model.SauBenh)
                .Set(x => x.GiaThe, model.GiaThe)
                .Set(x => x.TrongChau, model.TrongChau)
                .Set(x => x.TuoiNuoc, model.TuoiNuoc)
                .Set(x => x.BonPhan, model.BonPhan)
                .Set(x => x.CatTia, model.CatTia)
                .Set(x => x.ChamSoc, model.ChamSoc)                ;

            var result = _context.HOA.UpdateOneAsync(filter, update);
            if (result.Result.MatchedCount == 0)
            {

                throw new ResponseMessageException().WithException(DefaultCode.EXCEPTION);
            }
            return Hoa;
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

        public async Task<dynamic> GetByCode(string code)
        {
            try
            {
                var filter = Builders<HoaModel>.Filter.Eq("IsDeleted", false);
                filter = Builders<HoaModel>.Filter.And(filter, Builders<HoaModel>.Filter.Eq("Code", code));
                dynamic data = await _context.HOA.Find(filter).FirstOrDefaultAsync();
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

        public async Task<dynamic> GetPaging(PagingParam pagingParam)
        {
            PagingModel<dynamic> result = new PagingModel<dynamic>();
            var builder = Builders<HoaModel>.Filter;
            var filter = builder.Empty;
            filter = builder.And(filter, builder.Eq("IsDeleted", false));
            result.TotalRows = await _context.HOA.CountDocumentsAsync(filter);

            string sortBy = pagingParam.SortBy != null ? FormatterString.HandlerSortBy(pagingParam.SortBy) : "CreatedAt";
            result.Data = await _context.HOA.Find(filter)
                .SortByDescending(x=>x.CreatedAt)
                .ThenByDescending(e => e.CreatedAt)
                .Skip(pagingParam.Skip)
                .Limit(pagingParam.Limit)
                .ToListAsync();
            
                
            return result;
        }


        public async Task<dynamic> QRCode(string link)
        {
            try
            {               
                int width = 1000;
                int height = 1000;
                BarcodeWriter barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                barcodeWriter.Options = new ZXing.Common.EncodingOptions
                {
                    Width = width,
                    Height = height
                };

                Bitmap qrCode2Bitmap = barcodeWriter.Write(link);
                using (MemoryStream stream = new MemoryStream())
                {
                    qrCode2Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    var fileshort = _fileMinioService.UploadImg(stream, "home");
                return fileshort;
                }
               
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

        public async Task<dynamic> QRCode2()
        {
            try
            {
                List<HoaModel> Hoa = await _context.HOA.Find(x => !x.IsDeleted && !string.IsNullOrEmpty(x.Code)).ToListAsync();
                int width = 1000;
                int height = 1000;
                BarcodeWriter barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                barcodeWriter.Options = new ZXing.Common.EncodingOptions
                {
                    Width = width,
                    Height = height
                };
                foreach (var item in Hoa)
                {
                    Bitmap qrCode2Bitmap = barcodeWriter.Write("https://hoasadec.com.vn/hoa/chi-tiet/" + item.Code);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        qrCode2Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        var fileshort = _fileMinioService.UploadImg(stream, item.Name);
                        item.QRCode2 = fileshort.Result;
                    }
                    var update = Builders<HoaModel>.Update
                        .Set(x => x.QRCode2, item.QRCode2);
                    var filter = Builders<HoaModel>.Filter.Where(x => x.Id == item.Id);
                    var result = _context.HOA.UpdateOneAsync(filter, update);
                }

                return null;
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

        public async Task<dynamic> View()
        {
            try
            {
                List<HoaModel> Hoa = await _context.HOA.Find(x => !x.IsDeleted && !string.IsNullOrEmpty(x.Code)).ToListAsync();
                foreach (var item in Hoa)
                {
                    Random random = new Random();
                    int randomNumber = random.Next(1000, 1500);
                    var update = Builders<HoaModel>.Update
                        .Inc(x => x.View, randomNumber);
                    var filter = Builders<HoaModel>.Filter
                        .Where(x => x.Id == item.Id);
                    var result = _context.HOA.UpdateOneAsync(filter, update);
                }

                return null;
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
    }
}