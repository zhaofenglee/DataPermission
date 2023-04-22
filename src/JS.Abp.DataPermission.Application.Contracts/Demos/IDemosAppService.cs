using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.Demos
{
    public interface IDemosAppService : IApplicationService
    {
        Task<PagedResultDto<DemoDto>> GetListAsync(GetDemosInput input);

        Task<DemoDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<DemoDto> CreateAsync(DemoCreateDto input);

        Task<DemoDto> UpdateAsync(Guid id, DemoUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(DemoExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}