using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission.Blazor.Server;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(DataPermissionBlazorModule)
    )]
public class DataPermissionBlazorServerModule : AbpModule
{

}
