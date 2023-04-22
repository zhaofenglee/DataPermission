using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public interface IPermissionExtensionsAppService : IApplicationService
    {
        Task<PagedResultDto<PermissionExtensionDto>> GetListAsync(GetPermissionExtensionsInput input);

        Task<PermissionExtensionDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<PermissionExtensionDto> CreateAsync(PermissionExtensionCreateDto input);

        Task<PermissionExtensionDto> UpdateAsync(Guid id, PermissionExtensionUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(PermissionExtensionExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
        
        Task<List<PermissionRoleDto>> GetPermissionRoleListAsync();
    }
}