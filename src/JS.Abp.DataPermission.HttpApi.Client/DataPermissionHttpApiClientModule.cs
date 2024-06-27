using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace JS.Abp.DataPermission;

[DependsOn(
    typeof(DataPermissionApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class DataPermissionHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(DataPermissionApplicationContractsModule).Assembly,
            DataPermissionRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<DataPermissionHttpApiClientModule>();
        });

    }
}
