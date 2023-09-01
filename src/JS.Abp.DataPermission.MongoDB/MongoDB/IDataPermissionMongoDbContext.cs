using JS.Abp.DataPermission.Demos;
using JS.Abp.DataPermission.ObjectPermissions;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionItems;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace JS.Abp.DataPermission.MongoDB;

[ConnectionStringName(DataPermissionDbProperties.ConnectionStringName)]
public interface IDataPermissionMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<ObjectPermission> ObjectPermissions { get; }
    IMongoCollection<PermissionItem> PermissionItems { get; }

    IMongoCollection<Demo> Demos { get; }
    IMongoCollection<PermissionExtension> PermissionExtensions { get; }
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}