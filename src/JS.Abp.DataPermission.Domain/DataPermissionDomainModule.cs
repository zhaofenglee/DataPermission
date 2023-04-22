using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule),
    typeof(DataPermissionDomainSharedModule),
    typeof(AbpIdentityDomainModule)
)]
public class DataPermissionDomainModule : AbpModule
{

}
