using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace JS.Abp.DataPermission.Web.MongoDb.Data;

[ConnectionStringName("Default")]
public class MongoDbDbContext : AbpMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        //builder.Entity<YourEntity>(b =>
        //{
        //    //...
        //});
    }
}
