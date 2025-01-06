using Minio;
using Minio.DataModel.Args;
using MongoDB.Driver;
using FestivalHoa.Properties.Exceptions;
using FestivalHoa.Properties.Extensions;
using FestivalHoa.Properties.Helpers;
using FestivalHoa.Properties.Installers;
using FestivalHoa.Properties.Interfaces.Core;
using FestivalHoa.Properties.Models.Appsettings;
using FestivalHoa.Properties.Models.Core;
using File = FestivalHoa.Properties.Models.Core.FileModel;
using System.IO;


namespace FestivalHoa.Properties.Services.Core
{
    public class FileMinioService : BaseService, IFileMinioService
    {

        private readonly IMinioClient _minio;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private string _settings;
        private DataContext _context;
        private MinioClient _minioClient;
        private MinioSetting _minioSetting;
        private BaseMongoDb<File, string> BaseMongoDb;
        IMongoCollection<File> _collection;
        public FileMinioService(
            DataContext context,
            IWebHostEnvironment hostingEnvironment,
        
            IMinioClient minio,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            _minio = minio;
   

            _collection = context.FILES;
            _hostingEnvironment = hostingEnvironment;
            BaseMongoDb = new BaseMongoDb<File, string>(_context.FILES);
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();
            _minioSetting = _configuration.GetSection("MinioSettings").Get<MinioSetting>();
            _minioClient = (MinioClient)new MinioClient()
                .WithEndpoint(_minioSetting.End_Point)
                .WithCredentials(_minioSetting.Access_Key, _minioSetting.Secret_Key)
                .WithSSL()
                .Build();
            _settings = _configuration.GetSection("GetDomain").Get<string>();
            
        }

        public async Task<FileShort> UploadFile(IFormFile file)
        {
            var name = String.Empty;
            try
            {
                if (file == null || (file != null && file.Length == 0))
                    throw new ResponseMessageException().WithCode(DefaultCode.DATA_EXISTED).WithMessage(DefaultMessage.DATA_NOT_EMPTY);
                var currentDate = DateTime.UtcNow.ToLocalTime();
                var subfolderName = $"{currentDate.Day}-{currentDate.Month}-{currentDate.Year}";
                var path = $"{subfolderName}/";
                name = Guid.NewGuid().ToString();
                string fileExtension = Path.GetExtension(file.FileName);

                var stream = file.OpenReadStream();
                var request = new PutObjectArgs()
                    .WithBucket(_minioSetting.Bucket)
                    .WithObject(path + name + fileExtension)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType(_minioSetting.Content_Type);

                await _minioClient.PutObjectAsync(request);
                var result = await SaveFileAsyncMinio(file.FileName, name + fileExtension, path + name + fileExtension, path, file.Length, fileExtension, path);

                return result;
            }
            catch (Exception e)
            {
                throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
            }
        }


        public async Task<string> DeleteFile(string id)
        {
            var key = String.Empty;
            try
            {
                var file = _context.FILES.Find(x => x.Id == id && !x.IsDeleted).FirstOrDefault();
                if (file == null)
                    throw new ResponseMessageException().WithCode(DefaultCode.DATA_NOT_FOUND)
                        .WithMessage(DefaultMessage.DATA_NOT_FOUND);

                var filePath = file.Path;
                key = file.SaveName;
                var request = new RemoveObjectArgs().WithBucket(_minioSetting.Bucket).WithObject(filePath);
                await _minioClient.RemoveObjectAsync(request);

                file.IsDeleted = true;
                var result = await BaseMongoDb.UpdateAsync(file);
            }
            catch (Exception e)
            {
                throw new ResponseMessageException().WithException(DefaultCode.DELETE_FAILURE);
            }
            return key;
        }
        public async Task<byte[]> Dowload(string id)
        {
            try
            {
                var file = _context.FILES.Find(x => x.Id == id && !x.IsDeleted).FirstOrDefault();
                if (file == null)
                    throw new ResponseMessageException().WithException(DefaultCode.EXCEPTION);

                var filePath = file.Path;
                MemoryStream memoryStream = new MemoryStream();
                var localFilePath = Path.Combine(file.Path);
                var pathFile = Path.Combine(_hostingEnvironment.ContentRootPath, localFilePath);

                var request = new GetObjectArgs()
                    .WithBucket(_minioSetting.Bucket)
                    .WithObject(filePath)
                    .WithCallbackStream((data) => data.CopyTo(memoryStream));
                var a = await _minioClient.GetObjectAsync(request);
                // System.IO.File.WriteAllBytes($"{file.FileName}", memoryStream.ToArray());

                return memoryStream.ToArray();

            }
            catch (ResponseMessageException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<FileModel> GetById(string id)
        {
            return  _collection.Find(x => !x.IsDeleted && x.Id == id).FirstOrDefault();
        }
        public async Task<FileShort> SaveFileAsyncMinio(string fileName, string saveName, string path, string pathFolder, long fileSize, string fileExt, string foreignKey)
        {
            var entity = new File();
            entity.FileName = fileName;
            entity.SaveName = saveName;
            entity.Path = path;
            entity.PathFolder = pathFolder;
            entity.Size = fileSize;
            entity.Ext = fileExt;
            entity.CreatedAt = DateTime.Now;
            entity.IsDeleted = false;
            entity.Key = foreignKey;

            var result = await BaseMongoDb.CreateAsync(entity);

            if (result.Entity.Id == default || !result.Success)
                throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
            var fileshort = new FileShort();
            fileshort.FileId = entity.Id;
            fileshort.FileName = entity.FileName;
            fileshort.Ext = entity.Ext;
            fileshort.Path = entity.Path;
            return fileshort;
        }

        public async Task<FileShort> UploadImg(Stream stream, string name)
        {
            try
            {
               
                var currentDate = DateTime.UtcNow.ToLocalTime();
                var subfolderName = $"{currentDate.Day}-{currentDate.Month}-{currentDate.Year}";
                var path = $"QRCode/{subfolderName}/";
                string fileExtension = ".jpeg";
                stream.Seek(0, SeekOrigin.Begin);
                var request = new PutObjectArgs()
                    .WithBucket(_minioSetting.Bucket)
                    .WithObject(path + name + fileExtension)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType(_minioSetting.Content_Type);

                await _minioClient.PutObjectAsync(request);
                var result = await SaveFileAsyncMinio(name, name + fileExtension, path + name + fileExtension, path, stream.Length, fileExtension, path);

                return result;
            }
            catch (Exception e)
            {
                throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
            }
        }
        public async Task<string> UploadFileCK(IFormFile file)
        {
            var name = String.Empty;
            try
            {
                if (file == null || (file != null && file.Length == 0))
                    throw new ResponseMessageException()
                        .WithCode(DefaultCode.DATA_EXISTED)
                        .WithMessage(DefaultMessage.DATA_NOT_EMPTY);
                var currentDate = DateTime.UtcNow.ToLocalTime();
                var subfolderName = $"{currentDate.Day}-{currentDate.Month}-{currentDate.Year}";
                var path = $"{subfolderName}/";
                name = Guid.NewGuid().ToString();
                string fileExtension = Path.GetExtension(file.FileName);

                var stream = file.OpenReadStream();
                var request = new PutObjectArgs()
                    .WithBucket(_minioSetting.Bucket)
                    .WithObject(path + name + fileExtension)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType(_minioSetting.Content_Type);

                await _minioClient.PutObjectAsync(request);
                var result = await SaveFileAsyncMinio(file.FileName, name + fileExtension, path + name + fileExtension, path,  file.Length, fileExtension, path);

                return $"https://localhost:5001/api/v1/filesminio/view/{result.FileId}";;
            }
            catch (Exception e)
            {
                throw new ResponseMessageException().WithException(DefaultCode.CREATE_FAILURE);
            }
        }

    }
}