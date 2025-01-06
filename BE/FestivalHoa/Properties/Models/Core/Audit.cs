using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using FestivalHoa.Properties.Constants;

namespace FestivalHoa.Properties.Models.Core
{
    public class Audit
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        public string Name { get; set; } 

       public string UnsignedName
        {
            get { return Name != null ? FormatterString.ConvertToUnsign(Name) : ""; }
            set { }
        }
        
        [BsonDateTimeOptions]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        
        [BsonDateTimeOptions]
        public DateTime? ModifiedAt { get; set; } 
        
        public string CreatedBy { get; set; }
        
        public string ModifiedBy { get; set; }
        
        [BsonDefaultValue(false)]
        public bool IsDeleted { get; set; }
        
        [BsonIgnore]
        public string CreatedAtShow
        {
            get { return CreatedAt.HasValue ? CreatedAt.Value.ToLocalTime().ToString(FormatTime.FORMAT_DATE) : ""; }
        }
        
        [BsonIgnore]
        public string LastModifiedShow
        {
            get { return ModifiedAt.HasValue ? ModifiedAt.Value.ToLocalTime().ToString(FormatTime.FORMAT_DATE) : ""; }
        }
        
        public int? CreatedAtString
        {
            get { return CreatedAt.HasValue ? FormatDate.ConvertDatetimeQG(CreatedAt) : null; }
            set {}
        }
    }
    
}
