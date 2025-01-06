using FestivalHoa.Properties.Models.Core;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FestivalHoa.Properties.Models.CongDan
{
    public class DoanhNghiepModel : Audit, TEntity<string>
    {
        public string DiaChi {  get; set; }

        public string SDT {  get; set; }

        public string Content {  get; set; }
        
        public string Link { get; set; }

        //public FileShort QRCode { get; set; }
    }

    public class DoanhNghiepShort
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}

