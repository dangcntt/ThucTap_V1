using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.ViewModels
{
    public class DonViSelectedTreeVM
    {
        public DonViSelectedTreeVM(string id, string label)
        {
            this.Id = id;
            this.Label = label;
        }
        public DonViSelectedTreeVM(DonViTreeVM model)
        {
            this.Id = model.Id;
            this.Label = model.Name;
        }
        public DonViSelectedTreeVM(DonVi model)
        {
            this.Id = model.Id;
            this.Label = model.Name;
          //  this.Selected = false;
          //  this.Opened = false;
        }
        public string Id { get; set; }
        public string Label { get; set; }
        
        
        public List<DonViSelectedTreeVM> Children { get; set; }
    }
}