using JS.Abp.DataPermission.Demos;
using Volo.Abp.EntityFrameworkCore.Modeling;
using JS.Abp.DataPermission.PermissionExtensions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using JS.Abp.DataPermission.ObjectPermissions;
using JS.Abp.DataPermission.PermissionItems;

namespace JS.Abp.DataPermission.EntityFrameworkCore;

public static class DataPermissionDbContextModelCreatingExtensions
{
    public static void ConfigureDataPermission(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(DataPermissionDbProperties.DbTablePrefix + "Questions", DataPermissionDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */
        if (builder.IsHostDatabase())
        {
            builder.Entity<PermissionExtension>(b =>
{
    b.ToTable(DataPermissionDbProperties.DbTablePrefix + "PermissionExtensions", DataPermissionDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.ObjectName).HasColumnName(nameof(PermissionExtension.ObjectName)).IsRequired().HasMaxLength(PermissionExtensionConsts.ObjectNameMaxLength);
    b.Property(x => x.RoleId).HasColumnName(nameof(PermissionExtension.RoleId));
    b.Property(x => x.ExcludedRoleId).HasColumnName(nameof(PermissionExtension.ExcludedRoleId));
    b.Property(x => x.PermissionType).HasColumnName(nameof(PermissionExtension.PermissionType));
    b.Property(x => x.LambdaString).HasColumnName(nameof(PermissionExtension.LambdaString)).IsRequired();
    b.Property(x => x.IsActive).HasColumnName(nameof(PermissionExtension.IsActive));
    b.Property(x => x.Description).HasColumnName(nameof(PermissionExtension.Description)).HasMaxLength(PermissionExtensionConsts.DescriptionMaxLength);
});

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Demo>(b =>
{
    b.ToTable(DataPermissionDbProperties.DbTablePrefix + "Demos", DataPermissionDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.Name).HasColumnName(nameof(Demo.Name)).HasMaxLength(DemoConsts.NameMaxLength);
    b.Property(x => x.DisplayName).HasColumnName(nameof(Demo.DisplayName)).HasMaxLength(DemoConsts.DisplayNameMaxLength);
});

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<ObjectPermission>(b =>
            {
                b.ToTable(DataPermissionDbProperties.DbTablePrefix + "ObjectPermissions", DataPermissionDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.ObjectName).HasColumnName(nameof(ObjectPermission.ObjectName)).IsRequired().HasMaxLength(ObjectPermissionConsts.ObjectNameMaxLength);
                b.Property(x => x.Description).HasColumnName(nameof(ObjectPermission.Description));
            });

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<PermissionItem>(b =>
            {
                b.ToTable(DataPermissionDbProperties.DbTablePrefix + "PermissionItems", DataPermissionDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.ObjectName).HasColumnName(nameof(PermissionItem.ObjectName)).IsRequired().HasMaxLength(PermissionItemConsts.ObjectNameMaxLength);
                b.Property(x => x.Description).HasColumnName(nameof(PermissionItem.Description));
                b.Property(x => x.TargetType).HasColumnName(nameof(PermissionItem.TargetType)).IsRequired().HasMaxLength(PermissionItemConsts.TargetTypeMaxLength);
                b.Property(x=>x.RoleId).HasColumnName(nameof(PermissionItem.RoleId));
                b.Property(x => x.CanRead).HasColumnName(nameof(PermissionItem.CanRead));
                b.Property(x => x.CanCreate).HasColumnName(nameof(PermissionItem.CanCreate));
                b.Property(x => x.CanEdit).HasColumnName(nameof(PermissionItem.CanEdit));
                b.Property(x => x.CanDelete).HasColumnName(nameof(PermissionItem.CanDelete));
                b.Property(x => x.IsActive).HasColumnName(nameof(PermissionItem.IsActive));
            });

        }
    }
}