using Volo.Abp.Modularity;

namespace JS.Abp.DataPermission;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class DataPermissionDomainTestBase<TStartupModule> : DataPermissionTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
