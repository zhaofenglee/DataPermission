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
using JS.Abp.DataPermission.PermissionExtensions;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using JS.Abp.DataPermission.Shared;
using Volo.Abp.Identity;

namespace JS.Abp.DataPermission.PermissionExtensions
{

    [Authorize(DataPermissionPermissions.PermissionExtensions.Default)]
    public class PermissionExtensionsAppService : ApplicationService, IPermissionExtensionsAppService
    {
        private readonly IDistributedCache<PermissionExtensionExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IPermissionExtensionRepository _permissionExtensionRepository;
        private readonly PermissionExtensionManager _permissionExtensionManager;
        private readonly IIdentityRoleRepository _identityRoleRepository;

        public PermissionExtensionsAppService(IPermissionExtensionRepository permissionExtensionRepository, 
            PermissionExtensionManager permissionExtensionManager, 
            IDistributedCache<PermissionExtensionExcelDownloadTokenCacheItem, string> excelDownloadTokenCache,
            IIdentityRoleRepository identityRoleRepository)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _permissionExtensionRepository = permissionExtensionRepository;
            _permissionExtensionManager = permissionExtensionManager;
            _identityRoleRepository = identityRoleRepository;
        }

        public virtual async Task<PagedResultDto<PermissionExtensionDto>> GetListAsync(GetPermissionExtensionsInput input)
        {
            var totalCount = await _permissionExtensionRepository.GetCountAsync(input.FilterText, input.ObjectName, input.RoleId, input.ExcludedRoleId, input.PermissionType, input.LambdaString, input.IsActive);
            var items = await _permissionExtensionRepository.GetListAsync(input.FilterText, input.ObjectName, input.RoleId, input.ExcludedRoleId, input.PermissionType, input.LambdaString, input.IsActive, input.Sorting, input.MaxResultCount, input.SkipCount);
            //var permissionExtensionDtos = ObjectMapper.Map<List<PermissionExtension>, List<PermissionExtensionDto>>(items);
            var dtos = items.Select(queryResultItem =>
            {
                var dto = ObjectMapper.Map<PermissionExtension, PermissionExtensionDto>(queryResultItem);
                if (queryResultItem.RoleId!=Guid.Empty)
                {
                    dto.RoleName =  _identityRoleRepository.GetAsync(queryResultItem.RoleId).Result.Name;
                }
                
                return dto;
            }).ToList();
            return new PagedResultDto<PermissionExtensionDto>
            {
                TotalCount = totalCount,
                Items = dtos
            };
        }

        public virtual async Task<PermissionExtensionDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<PermissionExtension, PermissionExtensionDto>(await _permissionExtensionRepository.GetAsync(id));
        }

        [Authorize(DataPermissionPermissions.PermissionExtensions.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _permissionExtensionRepository.DeleteAsync(id);
        }

        [Authorize(DataPermissionPermissions.PermissionExtensions.Create)]
        public virtual async Task<PermissionExtensionDto> CreateAsync(PermissionExtensionCreateDto input)
        {

            var permissionExtension = await _permissionExtensionManager.CreateAsync(
            input.ObjectName, input.RoleId, input.PermissionType, input.LambdaString, input.IsActive, input.ExcludedRoleId
            );

            return ObjectMapper.Map<PermissionExtension, PermissionExtensionDto>(permissionExtension);
        }

        [Authorize(DataPermissionPermissions.PermissionExtensions.Edit)]
        public virtual async Task<PermissionExtensionDto> UpdateAsync(Guid id, PermissionExtensionUpdateDto input)
        {

            var permissionExtension = await _permissionExtensionManager.UpdateAsync(
            id,
            input.ObjectName, input.RoleId, input.PermissionType, input.LambdaString, input.IsActive, input.ExcludedRoleId, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<PermissionExtension, PermissionExtensionDto>(permissionExtension);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(PermissionExtensionExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _permissionExtensionRepository.GetListAsync(input.FilterText, input.ObjectName, input.RoleId, input.ExcludedRoleId, input.PermissionType, input.LambdaString, input.IsActive);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<PermissionExtension>, List<PermissionExtensionExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "PermissionExtensions.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new PermissionExtensionExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }

        public async Task<List<PermissionRoleDto>> GetPermissionRoleListAsync()
        {
            var items = await _identityRoleRepository.GetListAsync();
            return ObjectMapper.Map<List<IdentityRole>, List<PermissionRoleDto>>(items);
        }
    }
}