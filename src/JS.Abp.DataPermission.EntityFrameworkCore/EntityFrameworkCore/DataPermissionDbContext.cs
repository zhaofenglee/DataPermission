using JS.Abp.DataPermission.Demos;
using JS.Abp.DataPermission.PermissionExtensions;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using JS.Abp.DataPermission.PermissionItems;
using JS.Abp.DataPermission.ObjectPermissions;

namespace JS.Abp.DataPermission.EntityFrameworkCore;

[ConnectionStringName(DataPermissionDbProperties.ConnectionStringName)]
public class DataPermissionDbContext : AbpDbContext<DataPermissionDbContext>, IDataPermissionDbContext
{
    public DbSet<ObjectPermission> ObjectPermissions { get; set; }
    public DbSet<PermissionItem> PermissionItems { get; set; }
    public DbSet<Demo> Demos { get; set; }
    public DbSet<PermissionExtension> PermissionExtensions { get; set; }
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public DataPermissionDbContext(DbContextOptions<DataPermissionDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureDataPermission();
    }
}