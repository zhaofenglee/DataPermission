using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace JS.Abp.DataPermission.MongoDB;

[DependsOn(
    typeof(DataPermissionApplicationTestModule),
    typeof(DataPermissionMongoDbModule),
    typeof(AbpIdentityMongoDbModule)
)]
public class DataPermissionMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
        
        Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
        });
    }
}
