using JS.Abp.DataPermission.Demos;
using System;
using JS.Abp.DataPermission.Shared;
using JS.Abp.DataPermission.PermissionExtensions;
using Volo.Abp.Identity;
using JS.Abp.DataPermission.PermissionItems;
using JS.Abp.DataPermission.ObjectPermissions;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace JS.Abp.DataPermission;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionExtensionToPermissionExtensionDtoMapper : MapperBase<PermissionExtension, PermissionExtensionDto>
{
    [MapperIgnoreTarget(nameof(PermissionExtensionDto.RoleName))]
    public override partial PermissionExtensionDto Map(PermissionExtension source);

    [MapperIgnoreTarget(nameof(PermissionExtensionDto.RoleName))]
    public override partial void Map(PermissionExtension source, PermissionExtensionDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionExtensionToPermissionExtensionExcelDtoMapper : MapperBase<PermissionExtension, PermissionExtensionExcelDto>
{
    public override partial PermissionExtensionExcelDto Map(PermissionExtension source);
    public override partial void Map(PermissionExtension source, PermissionExtensionExcelDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionCacheItemToRowPermissionItemDtoMapper : MapperBase<PermissionCacheItem, Shared.RowPermissionItemDto>
{
    public override partial Shared.RowPermissionItemDto Map(PermissionCacheItem source);
    public override partial void Map(PermissionCacheItem source, Shared.RowPermissionItemDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class IdentityRoleToPermissionRoleDtoMapper : MapperBase<IdentityRole, PermissionRoleDto>
{
    public override partial PermissionRoleDto Map(IdentityRole source);
    public override partial void Map(IdentityRole source, PermissionRoleDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DemoToDemoDtoMapper : MapperBase<Demo, DemoDto>
{
    [MapperIgnoreTarget(nameof(DemoDto.Permission))]
    public override partial DemoDto Map(Demo source);

    [MapperIgnoreTarget(nameof(DemoDto.Permission))]
    public override partial void Map(Demo source, DemoDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DemoToDemoExcelDtoMapper : MapperBase<Demo, DemoExcelDto>
{
    public override partial DemoExcelDto Map(Demo source);
    public override partial void Map(Demo source, DemoExcelDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ObjectPermissionToObjectPermissionDtoMapper : MapperBase<ObjectPermission, ObjectPermissionDto>
{
    public override partial ObjectPermissionDto Map(ObjectPermission source);
    public override partial void Map(ObjectPermission source, ObjectPermissionDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ObjectPermissionToObjectPermissionExcelDtoMapper : MapperBase<ObjectPermission, ObjectPermissionExcelDto>
{
    public override partial ObjectPermissionExcelDto Map(ObjectPermission source);
    public override partial void Map(ObjectPermission source, ObjectPermissionExcelDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionItemToPermissionItemDtoMapper : MapperBase<PermissionItem, PermissionItems.PermissionItemDto>
{
    [MapperIgnoreTarget(nameof(PermissionItems.PermissionItemDto.RoleName))]
    public override partial PermissionItems.PermissionItemDto Map(PermissionItem source);

    [MapperIgnoreTarget(nameof(PermissionItems.PermissionItemDto.RoleName))]
    public override partial void Map(PermissionItem source, PermissionItems.PermissionItemDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionItemToPermissionItemExcelDtoMapper : MapperBase<PermissionItem, PermissionItemExcelDto>
{
    public override partial PermissionItemExcelDto Map(PermissionItem source);
    public override partial void Map(PermissionItem source, PermissionItemExcelDto destination);
}

