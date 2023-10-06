using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

[DependsOn(
    typeof(AbpAuthorizationModule)
)]
public class DataPermissionAuthorizationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IDataPermissionAuthorizationPolicyProvider, DataPermissionAuthorizationPolicyProvider>();
    }
}