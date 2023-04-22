using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(DataPermissionHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class DataPermissionConsoleApiClientModule : AbpModule
{

}
