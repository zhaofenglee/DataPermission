using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using JS.Abp.DataPermission.Permissions;
using JS.Abp.DataPermission.Demos;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.Demos
{

    [Authorize(DataPermissionPermissions.Demos.Default)]
    public class DemosAppService : ApplicationService, IDemosAppService
    {
        private readonly IDistributedCache<DemoExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IDemoRepository _demoRepository;
        private readonly DemoManager _demoManager;

        public DemosAppService(IDemoRepository demoRepository, DemoManager demoManager, IDistributedCache<DemoExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _demoRepository = demoRepository;
            _demoManager = demoManager;
        }

        public virtual async Task<PagedResultDto<DemoDto>> GetListAsync(GetDemosInput input)
        {
            var totalCount = await _demoRepository.GetCountAsync(input.FilterText, input.Name, input.DisplayName);
            var items = await _demoRepository.GetListAsync(input.FilterText, input.Name, input.DisplayName, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<DemoDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Demo>, List<DemoDto>>(items)
            };
        }

        public virtual async Task<DemoDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Demo, DemoDto>(await _demoRepository.GetAsync(id));
        }

        [Authorize(DataPermissionPermissions.Demos.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _demoRepository.DeleteAsync(id);
        }

        [Authorize(DataPermissionPermissions.Demos.Create)]
        public virtual async Task<DemoDto> CreateAsync(DemoCreateDto input)
        {

            var demo = await _demoManager.CreateAsync(
            input.Name, input.DisplayName
            );

            return ObjectMapper.Map<Demo, DemoDto>(demo);
        }

        [Authorize(DataPermissionPermissions.Demos.Edit)]
        public virtual async Task<DemoDto> UpdateAsync(Guid id, DemoUpdateDto input)
        {

            var demo = await _demoManager.UpdateAsync(
            id,
            input.Name, input.DisplayName, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Demo, DemoDto>(demo);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(DemoExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _demoRepository.GetListAsync(input.FilterText, input.Name, input.DisplayName);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Demo>, List<DemoExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Demos.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new DemoExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}