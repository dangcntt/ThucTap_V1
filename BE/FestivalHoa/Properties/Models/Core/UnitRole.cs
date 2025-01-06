using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FestivalHoa.Properties.Models.Core
{
    public class UnitRole : Audit , TEntity<string>
    {
        public string Code { get; set; }
        
        public int Sort { get; set; }
        
        public List<string> ListAction { get; set; } = new List<string>(); // Danh sách action của quyền  
        public List<string> ListMenu { get; set; } = new List<string>(); // Danh sách Menu mà quyền này có 
        
        public void SetAction(List<string> listAction)
        {
            ListMenu.Clear();
            ListAction.Clear();
            foreach (var item in  listAction)
            {
                if(item == null)
                    continue;
                var action = item.Split("-");
                if(action[1].Equals(""))
                    continue;
                var key = ListMenu.Where(x => x == action[1]).FirstOrDefault();
                if (key == null)
                {
                    ListMenu.Add(action[1]);
                }
            }
            ListAction = listAction;
        }
    }

    public class UnitRoleShort 
    {
        
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [JsonProperty(PropertyName = "_id")]
        public  string Id { get; set; }
        
        public string Name { get; set; }
        public string Code { get; set; }
        
        
        [BsonIgnore]
        public List<string> ListMenu { get; set; } = new List<string>(); // Danh sách Menu mà quyền này có 
        
        public UnitRoleShort() {}
        
        public UnitRoleShort(UnitRole unitRole)
        {
            this.Id = unitRole.Id;
            this.Code = unitRole.Code;
            this.Name = unitRole.Name;
            this.ListMenu = unitRole.ListMenu;
        }
        
        
    }
    
    
    
    
    public class UnitRoleShortShow
    {
        public  string Id { get; set; }
        
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsSpecialUnit { get; set; }

        public bool IsOnlySeeMe { get; set; } = true;
        
        public int Level { get; set; }
        
        
        
        public UnitRoleShortShow() {}

        
        public UnitRoleShortShow(UnitRole unitRole)
        {
            this.Id = unitRole.Id;
            this.Code = unitRole.Code;
            this.Name = unitRole.Name;
        }
        
        
    }
    
    

    
}