using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission.Blazor.WebAssembly;

[DependsOn(
    typeof(DataPermissionBlazorModule),
    typeof(DataPermissionHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
)]
public class DataPermissionBlazorWebAssemblyModule : AbpModule
{

}
