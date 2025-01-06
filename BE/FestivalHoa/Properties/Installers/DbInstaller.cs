
using FestivalHoa.Properties.Interfaces.Auth;
using FestivalHoa.Properties.Interfaces.Common;
using FestivalHoa.Properties.Interfaces.Core;
using FestivalHoa.Properties.Interfaces.NghiepVu;
using FestivalHoa.Properties.Services.Auth;
using FestivalHoa.Properties.Services.Common;
using FestivalHoa.Properties.Services.Core;
using FestivalHoa.Properties.Services.NghiepVu;

namespace FestivalHoa.Properties.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<DataContext>();

            #region Auth 
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IIdentityService, IdentityService>();
            #endregion

            #region Common

            services.AddScoped<ICommonService, CommonService>();

            #endregion
            
            #region Core 

            services.AddScoped<IUnitRoleService, UnitRoleService>();
            


            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMenuCongDanService, MenuCongDanService>();
     //     services.AddScoped<IModuleService, ModuleService>();

         

            services.AddScoped<IUserService, UserService>();



           
       
           
           
            
            services.AddScoped<IFileMinioService, FileMinioService>();

            #endregion


            #region Nghiệp vụ  

            services.AddScoped<IDoanhNghiepService, DoanhNghiepService>();
            services.AddScoped<IHoaService, HoaService>();
            #endregion


        }
    }
}
