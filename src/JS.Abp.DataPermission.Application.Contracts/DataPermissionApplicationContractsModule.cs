using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission;

[DependsOn(
    typeof(DataPermissionDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule)
    )]
public class DataPermissionApplicationContractsModule : AbpModule
{

}
