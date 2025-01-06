using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.ViewModels
{
    public class DonViTreeVM
    {
        public DonViTreeVM(string id, string label)
        {
            this.Id = id;
            this.Name = label;
        }
        public DonViTreeVM(DonViTreeVM model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
        }
        public DonViTreeVM(DonVi model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
          //  this.Selected = false;
          //  this.Opened = false;
        }
        
        public string Id { get; set; }
        public string Name { get; set; }
        
        
        public List<DonViTreeVM> Children { get; set; }
    }
    
    
    public class DonViTreeVMGetAll
    {
        public DonViTreeVMGetAll(string id, string label)
        {
            this.Id = id;
            this.Name = label;
        }
        public DonViTreeVMGetAll(DonViTreeVM model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
        }
        public DonViTreeVMGetAll(DonVi model)
        {
            this.Id = model.Id;
            this.Name = model.Name;
            this.Code = model.MaCoQuan; 
            this.CapDV = model.CapDV;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        
        public string Code { get; set; }
        
        public int CapDV { get; set; }
        
    }
}