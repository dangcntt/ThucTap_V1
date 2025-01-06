using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using FestivalHoa.Properties.Interfaces.Core;

namespace FestivalHoa.Properties.Models.Core;

public class CommonModel:Audit, IIdEntity<string>, TEntity<string>
{

    public string Code { get; set; } // Code của hệ thống mới của mình 
    
    
  

   // public string FullName => $"{Name} khong : " + $"{Code}"; 
    // public bool Bool => !string.IsNullOrEmpty(Name);

    public int Sort { get; set; }

    [BsonIgnore] public string CollectionName { get; set; }
}





public class CommonModelShort
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [JsonProperty(PropertyName = "_id")]
    public string Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public CommonModelShort()
    {

    }

    public CommonModelShort(string id, string code, string name)
    {
        this.Id = id;
        this.Code = code;
        this.Name = name; 
    }

    // public string FullName => $"{Name}  : " + $"{Code}"; 
    // public bool Bool => !string.IsNullOrEmpty(Name);
}