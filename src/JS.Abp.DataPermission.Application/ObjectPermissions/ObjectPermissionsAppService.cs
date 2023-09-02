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
using JS.Abp.DataPermission.ObjectPermissions;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.ObjectPermissions
{

    [Authorize(DataPermissionPermissions.ObjectPermissions.Default)]
    public class ObjectPermissionsAppService : ApplicationService, IObjectPermissionsAppService
    {
        private readonly IDistributedCache<ObjectPermissionExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IObjectPermissionRepository _objectPermissionRepository;
        private readonly ObjectPermissionManager _objectPermissionManager;

        public ObjectPermissionsAppService(IObjectPermissionRepository objectPermissionRepository, ObjectPermissionManager objectPermissionManager, IDistributedCache<ObjectPermissionExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _objectPermissionRepository = objectPermissionRepository;
            _objectPermissionManager = objectPermissionManager;
        }

        public virtual async Task<PagedResultDto<ObjectPermissionDto>> GetListAsync(GetObjectPermissionsInput input)
        {
            var totalCount = await _objectPermissionRepository.GetCountAsync(input.FilterText, input.ObjectName, input.Description);
            var items = await _objectPermissionRepository.GetListAsync(input.FilterText, input.ObjectName, input.Description, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ObjectPermissionDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ObjectPermission>, List<ObjectPermissionDto>>(items)
            };
        }

        public virtual async Task<ObjectPermissionDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<ObjectPermission, ObjectPermissionDto>(await _objectPermissionRepository.GetAsync(id));
        }

        [Authorize(DataPermissionPermissions.ObjectPermissions.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _objectPermissionRepository.DeleteAsync(id);
        }

        [Authorize(DataPermissionPermissions.ObjectPermissions.Create)]
        public virtual async Task<ObjectPermissionDto> CreateAsync(ObjectPermissionCreateDto input)
        {

            var objectPermission = await _objectPermissionManager.CreateAsync(
             input.ObjectName, input.Description
            );

            return ObjectMapper.Map<ObjectPermission, ObjectPermissionDto>(objectPermission);
        }

        [Authorize(DataPermissionPermissions.ObjectPermissions.Edit)]
        public virtual async Task<ObjectPermissionDto> UpdateAsync(Guid id, ObjectPermissionUpdateDto input)
        {

            var objectPermission = await _objectPermissionManager.UpdateAsync(
            id,
             input.ObjectName, input.Description, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<ObjectPermission, ObjectPermissionDto>(objectPermission);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ObjectPermissionExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _objectPermissionRepository.GetListAsync(input.FilterText, input.ObjectName, input.Description);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<ObjectPermission>, List<ObjectPermissionExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "ObjectPermissions.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new ObjectPermissionExcelDownloadTokenCacheItem { Token = token },
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