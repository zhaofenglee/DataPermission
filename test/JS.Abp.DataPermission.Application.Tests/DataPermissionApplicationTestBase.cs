using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class DataPermissionApplicationTestBase<TStartupModule> : DataPermissionTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
