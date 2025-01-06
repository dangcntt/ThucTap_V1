using FestivalHoa.Properties.Models.Core;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace FestivalHoa.Properties.Models.CongDan
{
    public class HoaModel : Audit, TEntity<string>
    {
        public FileShort Avatar {  get; set; }
        public CommonModelShort PhanLoai {  get; set; }

        public string NoiDung {  get; set; }
        
        public string MoTa { get; set; }

        public List<string> QuaTrinhCanhTac {  get; set; }
        
        public List<FileShort> Files { get; set; }

        public FileShort QRCode { get; set; }

        public FileShort QRCode2 { get; set; }

        public List<DoanhNghiepShort> DoanhNghiep { get; set; }

        public string Code { get; set; }

        public int Sort { get; set; }

        public string EngName { get; set; }

        public string CayGiong { get; set; }

        public string SauBenh { get; set; }

        public string GiaThe { get; set; }

        public string TrongChau { get; set; }

        public string TuoiNuoc { get; set; }

        public string BonPhan { get; set; }

        public string CatTia { get; set; }

       public List<ChamSoc> ChamSoc { get; set; }

        public int View {  get; set; }
    }

    public class ChamSoc
    {
        public string Name { get; set; }
        public FileShort File { get; set; }
    }

}

