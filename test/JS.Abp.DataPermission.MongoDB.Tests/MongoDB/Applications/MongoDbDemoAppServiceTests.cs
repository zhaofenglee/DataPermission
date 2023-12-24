using JS.Abp.DataPermission.Demos;
using Xunit;

namespace JS.Abp.DataPermission.MongoDB.Applications;

[Collection(MongoTestCollection.Name)]
public class MongoDbDemoAppServiceTests:  DemosAppServiceTests<DataPermissionMongoDbTestModule>
{
    
}