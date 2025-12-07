using JS.Abp.DataPermission.Web.Pages.DataPermission.Demos;
using JS.Abp.DataPermission.Demos;
using JS.Abp.DataPermission.Web.Pages.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.ObjectPermissions;
using JS.Abp.DataPermission.PermissionItems;
using JS.Abp.DataPermission.Web.Pages.DataPermission.ObjectPermissions;
using JS.Abp.DataPermission.Web.Pages.DataPermission.PermissionItems;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace JS.Abp.DataPermission.Web;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionExtensionDtoToPermissionExtensionUpdateViewModelMapper : MapperBase<PermissionExtensionDto, PermissionExtensionUpdateViewModel>
{
    public override partial PermissionExtensionUpdateViewModel Map(PermissionExtensionDto source);
    public override partial void Map(PermissionExtensionDto source, PermissionExtensionUpdateViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionExtensionUpdateViewModelToPermissionExtensionUpdateDtoMapper : MapperBase<PermissionExtensionUpdateViewModel, PermissionExtensionUpdateDto>
{
    public override partial PermissionExtensionUpdateDto Map(PermissionExtensionUpdateViewModel source);
    public override partial void Map(PermissionExtensionUpdateViewModel source, PermissionExtensionUpdateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionExtensionCreateViewModelToPermissionExtensionCreateDtoMapper : MapperBase<PermissionExtensionCreateViewModel, PermissionExtensionCreateDto>
{
    public override partial PermissionExtensionCreateDto Map(PermissionExtensionCreateViewModel source);
    public override partial void Map(PermissionExtensionCreateViewModel source, PermissionExtensionCreateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DemoDtoToDemoUpdateViewModelMapper : MapperBase<DemoDto, DemoUpdateViewModel>
{
    public override partial DemoUpdateViewModel Map(DemoDto source);
    public override partial void Map(DemoDto source, DemoUpdateViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DemoUpdateViewModelToDemoUpdateDtoMapper : MapperBase<DemoUpdateViewModel, DemoUpdateDto>
{
    public override partial DemoUpdateDto Map(DemoUpdateViewModel source);
    public override partial void Map(DemoUpdateViewModel source, DemoUpdateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DemoCreateViewModelToDemoCreateDtoMapper : MapperBase<DemoCreateViewModel, DemoCreateDto>
{
    public override partial DemoCreateDto Map(DemoCreateViewModel source);
    public override partial void Map(DemoCreateViewModel source, DemoCreateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionItemDtoToPermissionItemUpdateViewModelMapper : MapperBase<PermissionItemDto, PermissionItemUpdateViewModel>
{
    public override partial PermissionItemUpdateViewModel Map(PermissionItemDto source);
    public override partial void Map(PermissionItemDto source, PermissionItemUpdateViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionItemUpdateViewModelToPermissionItemUpdateDtoMapper : MapperBase<PermissionItemUpdateViewModel, PermissionItemUpdateDto>
{
    public override partial PermissionItemUpdateDto Map(PermissionItemUpdateViewModel source);
    public override partial void Map(PermissionItemUpdateViewModel source, PermissionItemUpdateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionItemCreateViewModelToPermissionItemCreateDtoMapper : MapperBase<PermissionItemCreateViewModel, PermissionItemCreateDto>
{
    public override partial PermissionItemCreateDto Map(PermissionItemCreateViewModel source);
    public override partial void Map(PermissionItemCreateViewModel source, PermissionItemCreateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ObjectPermissionDtoToObjectPermissionUpdateViewModelMapper : MapperBase<ObjectPermissionDto, ObjectPermissionUpdateViewModel>
{
    public override partial ObjectPermissionUpdateViewModel Map(ObjectPermissionDto source);
    public override partial void Map(ObjectPermissionDto source, ObjectPermissionUpdateViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ObjectPermissionUpdateViewModelToObjectPermissionUpdateDtoMapper : MapperBase<ObjectPermissionUpdateViewModel, ObjectPermissionUpdateDto>
{
    public override partial ObjectPermissionUpdateDto Map(ObjectPermissionUpdateViewModel source);
    public override partial void Map(ObjectPermissionUpdateViewModel source, ObjectPermissionUpdateDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ObjectPermissionCreateViewModelToObjectPermissionCreateDtoMapper : MapperBase<ObjectPermissionCreateViewModel, ObjectPermissionCreateDto>
{
    public override partial ObjectPermissionCreateDto Map(ObjectPermissionCreateViewModel source);
    public override partial void Map(ObjectPermissionCreateViewModel source, ObjectPermissionCreateDto destination);
}

