using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.Interfaces.Core
{
    public interface IFileMinioService
    {
        public Task<string> UploadFileCK(IFormFile file);
        public Task<FileShort> UploadFile(IFormFile file);
        public Task<string> DeleteFile(string id);
        public Task<byte[]> Dowload(string id);
        public Task<FileModel> GetById(string id);

        public Task<FileShort> UploadImg(Stream stream, string name);
    }
}