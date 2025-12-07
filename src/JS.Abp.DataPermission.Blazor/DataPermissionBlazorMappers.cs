using JS.Abp.DataPermission.Demos;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.ObjectPermissions;
using JS.Abp.DataPermission.PermissionItems;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace JS.Abp.DataPermission.Blazor;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionExtensionDtoToPermissionExtensionUpdateDtoMapper : MapperBase<PermissionExtensionDto, PermissionExtensionUpdateDto>
{
    public override partial PermissionExtensionUpdateDto Map(PermissionExtensionDto source);
    public override partial void Map(PermissionExtensionDto source, PermissionExtensionUpdateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DemoDtoToDemoUpdateDtoMapper : MapperBase<DemoDto, DemoUpdateDto>
{
    public override partial DemoUpdateDto Map(DemoDto source);
    public override partial void Map(DemoDto source, DemoUpdateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionItemDtoToPermissionItemUpdateDtoMapper : MapperBase<PermissionItemDto, PermissionItemUpdateDto>
{
    public override partial PermissionItemUpdateDto Map(PermissionItemDto source);
    public override partial void Map(PermissionItemDto source, PermissionItemUpdateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ObjectPermissionDtoToObjectPermissionUpdateDtoMapper : MapperBase<ObjectPermissionDto, ObjectPermissionUpdateDto>
{
    public override partial ObjectPermissionUpdateDto Map(ObjectPermissionDto source);
    public override partial void Map(ObjectPermissionDto source, ObjectPermissionUpdateDto destination);
}

