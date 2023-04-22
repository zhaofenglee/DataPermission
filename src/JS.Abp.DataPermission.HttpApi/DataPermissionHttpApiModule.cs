using Localization.Resources.AbpUi;
using JS.Abp.DataPermission.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace JS.Abp.DataPermission;

[DependsOn(
    typeof(DataPermissionApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class DataPermissionHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(DataPermissionHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<DataPermissionResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
