using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public interface IObjectPermissionsAppService : IApplicationService
    {
        Task<PagedResultDto<ObjectPermissionDto>> GetListAsync(GetObjectPermissionsInput input);

        Task<ObjectPermissionDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<ObjectPermissionDto> CreateAsync(ObjectPermissionCreateDto input);

        Task<ObjectPermissionDto> UpdateAsync(Guid id, ObjectPermissionUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ObjectPermissionExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}