using JS.Abp.DataPermission.Demos;
using JS.Abp.DataPermission.PermissionExtensions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace JS.Abp.DataPermission.MongoDB;

[DependsOn(
    typeof(DataPermissionDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class DataPermissionMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<DataPermissionMongoDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, MongoQuestionRepository>();
             */
            options.AddRepository<PermissionExtension, PermissionExtensions.MongoPermissionExtensionRepository>();

            options.AddRepository<Demo, Demos.MongoDemoRepository>();

        });
    }
}