using JS.Abp.DataPermission.Demos;
using System;
using JS.Abp.DataPermission.Shared;
using Volo.Abp.AutoMapper;
using JS.Abp.DataPermission.PermissionExtensions;
using AutoMapper;
using Volo.Abp.Identity;
using JS.Abp.DataPermission.PermissionItems;
using JS.Abp.DataPermission.ObjectPermissions;

namespace JS.Abp.DataPermission;

public class DataPermissionApplicationAutoMapperProfile : Profile
{
    public DataPermissionApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<PermissionExtension, PermissionExtensionDto>()
            .ForMember(dest => dest.RoleName, opt => opt.Ignore());
        CreateMap<PermissionExtension, PermissionExtensionExcelDto>();
        CreateMap<PermissionCacheItem, Shared.RowPermissionItemDto>();
        CreateMap<IdentityRole, PermissionRoleDto>();
        
        CreateMap<Demo, DemoDto>()
            .ForMember(dest => dest.Permission, opt => opt.Ignore());
        CreateMap<Demo, DemoExcelDto>();

        CreateMap<ObjectPermission, ObjectPermissionDto>();
        CreateMap<ObjectPermission, ObjectPermissionExcelDto>();

        CreateMap<PermissionItem, PermissionItems.PermissionItemDto>()
            .ForMember(dest => dest.RoleName, opt => opt.Ignore());
        CreateMap<PermissionItem, PermissionItemExcelDto>();

    }
}