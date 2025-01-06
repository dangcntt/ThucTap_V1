using MongoDB.Driver;
using FestivalHoa.Properties.Models.Core;

namespace FestivalHoa.Properties.Constants;

public class Projection
{

    public static ProjectionDefinition<CommonModel> Projection_BasicCommon = Builders<CommonModel>.Projection
        .Include(x=>x.Id)
        .Include(x=>x.Code)
        .Include(x =>x.Name);
    



}