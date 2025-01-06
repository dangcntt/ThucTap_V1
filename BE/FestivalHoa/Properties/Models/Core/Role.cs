namespace FestivalHoa.Properties.Models.Core
{
    public class Role: Audit, TEntity<string>
    {
        public string Code { get; set; }
        public string Ten { get; set; }
        public int ThuTu { get; set; }
        
        public List<Menu> Menus { get; set; } = new List<Menu>();
        public List<string> Permissions { get; set; } = new List<string>();


    }
}