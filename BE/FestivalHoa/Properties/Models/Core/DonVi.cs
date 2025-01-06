using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FestivalHoa.Properties.Models.Core
{
    public class DonVi : Audit, TEntity<string>
    {
        public string MaCoQuan { get; set; }

        public string DonViCha { get; set; }
        
        public string MaCoQuanCha { get; set; }
        
        public int CapDV { get; set; }
        
        public int Sort { get; set; }
    }

    public class DonViShort 
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        public string Ten { get; set; } 
        public string MaCoQuan { get; set; }
        
        public DonViShort()
        {
        }
        public DonViShort(string id , string ten)
        {
            this.Id = id;
            this.Ten = ten;
        }
    }
}