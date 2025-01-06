namespace FestivalHoa.Properties.Models.Core
{
    public class FileModel : Audit, TEntity<string>
    {
        
        public string FileName { get; set; }
        public string SaveName { get; set; }
        public string Path { get; set; }
        
        public string PathFolder { get; set; }
        
        public string Group  { get; set; } // File thuoc nhom thong tin nao 
        public long Size { get; set; }
        public string Ext { get; set; }
        public string Key { get; set; }
    }

    public class FileShort
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        
        public  string Ext { get; set;  }
        public string Path { get; set; }
    }
}