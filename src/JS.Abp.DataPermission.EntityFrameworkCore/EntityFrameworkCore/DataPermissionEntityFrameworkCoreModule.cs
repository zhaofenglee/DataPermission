using JS.Abp.DataPermission.Demos;
using JS.Abp.DataPermission.ObjectPermissions;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionItems;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission.EntityFrameworkCore;

[DependsOn(
    typeof(DataPermissionDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class DataPermissionEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<DataPermissionDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddRepository<PermissionExtension, PermissionExtensions.EfCorePermissionExtensionRepository>();

            options.AddRepository<Demo, Demos.EfCoreDemoRepository>();
            
            options.AddRepository<ObjectPermission, ObjectPermissions.EfCoreObjectPermissionRepository>();
            
            options.AddRepository<PermissionItem, PermissionItems.EfCorePermissionItemRepository>();

        });
    }
}