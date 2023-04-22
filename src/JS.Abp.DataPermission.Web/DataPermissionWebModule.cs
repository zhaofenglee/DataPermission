using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using JS.Abp.DataPermission.Localization;
using JS.Abp.DataPermission.Web.Menus;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using JS.Abp.DataPermission.Permissions;

namespace JS.Abp.DataPermission.Web;

[DependsOn(
    typeof(DataPermissionApplicationContractsModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAutoMapperModule)
    )]
public class DataPermissionWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(DataPermissionResource), typeof(DataPermissionWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(DataPermissionWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new DataPermissionMenuContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<DataPermissionWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<DataPermissionWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<DataPermissionWebModule>(validate: true);
        });

        Configure<RazorPagesOptions>(options =>
        {
            //Configure authorization.
            options.Conventions.AuthorizePage("/PermissionExtensions/Index", DataPermissionPermissions.PermissionExtensions.Default);
            options.Conventions.AuthorizePage("/Demos/Index", DataPermissionPermissions.Demos.Default);
        });
    }
}