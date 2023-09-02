using JS.Abp.DataPermission.Demos;
using JS.Abp.DataPermission.PermissionExtensions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using JS.Abp.DataPermission.PermissionItems;
using JS.Abp.DataPermission.ObjectPermissions;

namespace JS.Abp.DataPermission.EntityFrameworkCore;

[ConnectionStringName(DataPermissionDbProperties.ConnectionStringName)]
public interface IDataPermissionDbContext : IEfCoreDbContext
{
    DbSet<ObjectPermission> ObjectPermissions { get; set; }
    DbSet<PermissionItem> PermissionItems { get; set; }
    DbSet<Demo> Demos { get; set; }
    DbSet<PermissionExtension> PermissionExtensions { get; set; }
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}