using Volo.Abp;
using Volo.Abp.MongoDB;

namespace JS.Abp.DataPermission.MongoDB;

public static class DataPermissionMongoDbContextExtensions
{
    public static void ConfigureDataPermission(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
