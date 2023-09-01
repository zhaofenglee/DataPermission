using JS.Abp.DataPermission.Demos;
using JS.Abp.DataPermission.ObjectPermissions;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionItems;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace JS.Abp.DataPermission.MongoDB;

[ConnectionStringName(DataPermissionDbProperties.ConnectionStringName)]
public class DataPermissionMongoDbContext : AbpMongoDbContext, IDataPermissionMongoDbContext
{
    public IMongoCollection<ObjectPermission> ObjectPermissions => Collection<ObjectPermission>();
    public IMongoCollection<PermissionItem> PermissionItems => Collection<PermissionItem>();

    public IMongoCollection<Demo> Demos => Collection<Demo>();
    public IMongoCollection<PermissionExtension> PermissionExtensions => Collection<PermissionExtension>();
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureDataPermission();

        modelBuilder.Entity<PermissionExtension>(b => { b.CollectionName = DataPermissionDbProperties.DbTablePrefix + "PermissionExtensions"; });

        modelBuilder.Entity<Demo>(b => { b.CollectionName = DataPermissionDbProperties.DbTablePrefix + "Demos"; });
        modelBuilder.Entity<PermissionItem>(b => { b.CollectionName = DataPermissionDbProperties.DbTablePrefix + "PermissionItems"; });

        modelBuilder.Entity<ObjectPermission>(b => { b.CollectionName = DataPermissionDbProperties.DbTablePrefix + "ObjectPermissions"; });

    }
}