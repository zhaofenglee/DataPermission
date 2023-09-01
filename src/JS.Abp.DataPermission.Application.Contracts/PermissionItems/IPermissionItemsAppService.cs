using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.PermissionItems
{
    public interface IPermissionItemsAppService : IApplicationService
    {
        Task<PagedResultDto<PermissionItemDto>> GetListAsync(GetPermissionItemsInput input);

        Task<PermissionItemDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<PermissionItemDto> CreateAsync(PermissionItemCreateDto input);
        
        Task<PermissionItemDto> CopyAsync(Guid id);
        Task<PermissionItemDto> UpdateAsync(Guid id, PermissionItemUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(PermissionItemExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}