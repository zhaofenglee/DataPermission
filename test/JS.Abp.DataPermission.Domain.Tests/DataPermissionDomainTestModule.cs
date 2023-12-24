using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission;

[DependsOn(
    typeof(DataPermissionDomainModule),
    typeof(DataPermissionTestBaseModule)
)]
public class DataPermissionDomainTestModule : AbpModule
{

}
