using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission;

[DependsOn(
    typeof(DataPermissionApplicationModule),
    typeof(DataPermissionDomainTestModule)
    )]
public class DataPermissionApplicationTestModule : AbpModule
{

}
