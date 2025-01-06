using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.ViewModels
{
    public class MenuTreeAndActionVM
    {
     
        public MenuTreeAndActionVM(Menu model)
        {
            this.Id = model.Id;
            this.Label = model.Name;
            this.Link = model.Path ?? "";
            this.Icon = model.Icon ?? "";
            this.ParentId = model.ParentId;
            this.ListAction = model.ListAction;
        }
        public string Id { get; set; }
        public string Label { get; set; }
        public List<MenuTreeAndActionVM> Children { get; set; }
        
        public List<string> ListAction
        {
            get;
            set;
        } = new List<string>();
        public bool Opened { get; set; } = false;
        public string ParentId { get; set; }= "";
        public string Link { get; set; } = "";
        public string Icon { get; set; } = "";
        public bool Selected { get; set; } = false;
    }

}