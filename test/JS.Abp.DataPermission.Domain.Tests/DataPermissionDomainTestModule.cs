using JS.Abp.DataPermission.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(DataPermissionEntityFrameworkCoreTestModule)
    )]
public class DataPermissionDomainTestModule : AbpModule
{

}
