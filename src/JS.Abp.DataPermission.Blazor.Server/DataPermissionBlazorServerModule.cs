using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission.Blazor.Server;

[DependsOn(
    typeof(DataPermissionBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule)
    )]
public class DataPermissionBlazorServerModule : AbpModule
{

}
