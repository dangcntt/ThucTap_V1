using MongoDB.Driver;

namespace FestivalHoa.Properties.Models.Core
{
    public class Module : Audit, TEntity<string>
    {

        public List<string> ListAction { get; set; }

    }

    public class ModuleShort
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> ListAction { get; set; }
    }
    public class MenuModuel
    {
        public static ProjectionDefinition<Menu> Projection_BasicCommon = Builders<Menu>.Projection
        .Include(x => x.Id)
        .Include(x => x.Name)
        .Include(x => x.ListAction);
    }
}
