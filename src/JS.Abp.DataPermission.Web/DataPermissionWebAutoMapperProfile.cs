using JS.Abp.DataPermission.Web.Pages.DataPermission.Demos;
using JS.Abp.DataPermission.Demos;
using JS.Abp.DataPermission.Web.Pages.DataPermission.PermissionExtensions;
using Volo.Abp.AutoMapper;
using JS.Abp.DataPermission.PermissionExtensions;
using AutoMapper;

namespace JS.Abp.DataPermission.Web;

public class DataPermissionWebAutoMapperProfile : Profile
{
    public DataPermissionWebAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<PermissionExtensionDto, PermissionExtensionUpdateViewModel>();
        CreateMap<PermissionExtensionUpdateViewModel, PermissionExtensionUpdateDto>();
        CreateMap<PermissionExtensionCreateViewModel, PermissionExtensionCreateDto>();

        CreateMap<DemoDto, DemoUpdateViewModel>();
        CreateMap<DemoUpdateViewModel, DemoUpdateDto>();
        CreateMap<DemoCreateViewModel, DemoCreateDto>();
    }
}