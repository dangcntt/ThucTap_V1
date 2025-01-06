using FestivalHoa.Properties.Installers;
using FestivalHoa.Properties.Interfaces.Common;
using FestivalHoa.Properties.Models.Core;
using FestivalHoa.Properties.Services.Core;

namespace FestivalHoa.Properties.Services.Common;

public class CommonService: CommmonRepository<CommonModel, string>, ICommonService
{
       
    public CommonService(DataContext context, IHttpContextAccessor contextAccessor) :
        base(context, contextAccessor)
    {
    }
    
        
}