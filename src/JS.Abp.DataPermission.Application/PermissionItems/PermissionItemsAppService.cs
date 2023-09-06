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
using JS.Abp.DataPermission.PermissionItems;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using JS.Abp.DataPermission.Shared;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.Identity;

namespace JS.Abp.DataPermission.PermissionItems
{

    [Authorize(DataPermissionPermissions.PermissionItems.Default)]
    public class PermissionItemsAppService : ApplicationService, IPermissionItemsAppService
    {
        private readonly IDistributedCache<PermissionItemExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IPermissionItemRepository _permissionItemRepository;
        private readonly PermissionItemManager _permissionItemManager;
        private readonly IEntityCache<IdentityRole, Guid> _roleCache;
        public PermissionItemsAppService(IPermissionItemRepository permissionItemRepository, 
            PermissionItemManager permissionItemManager,
            IDistributedCache<PermissionItemExcelDownloadTokenCacheItem, string> excelDownloadTokenCache,
            IEntityCache<IdentityRole, Guid> roleCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _permissionItemRepository = permissionItemRepository;
            _permissionItemManager = permissionItemManager;
            _roleCache = roleCache;
        }

        public virtual async Task<PagedResultDto<PermissionItemDto>> GetListAsync(GetPermissionItemsInput input)
        {
            var totalCount = await _permissionItemRepository.GetCountAsync(input.FilterText, input.ObjectName, input.Description, input.TargetType,input.RoleId, input.CanRead, input.CanCreate, input.CanEdit, input.CanDelete,input.IsActive);
            var items = await _permissionItemRepository.GetListAsync(input.FilterText, input.ObjectName, input.Description, input.TargetType,input.RoleId, input.CanRead, input.CanCreate, input.CanEdit, input.CanDelete,input.IsActive, input.Sorting, input.MaxResultCount, input.SkipCount);
            var dtos = await Task.WhenAll(items.Select(async queryResultItem =>
            {
                var dto = ObjectMapper.Map<PermissionItem, PermissionItemDto>(queryResultItem);
                if (queryResultItem.RoleId != Guid.Empty)
                {
                    var role = await _roleCache.FindAsync(queryResultItem.RoleId);
                    dto.RoleName = role?.Name;
                }

                return dto;
            }));
            
            return new PagedResultDto<PermissionItemDto>
            {
                TotalCount = totalCount,
                Items = dtos
            };
        }

        public virtual async Task<PermissionItemDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<PermissionItem, PermissionItemDto>(await _permissionItemRepository.GetAsync(id));
        }

        [Authorize(DataPermissionPermissions.PermissionItems.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _permissionItemRepository.DeleteAsync(id);
        }

        [Authorize(DataPermissionPermissions.PermissionItems.Create)]
        public virtual async Task<PermissionItemDto> CreateAsync(PermissionItemCreateDto input)
        {

            var permissionItem = await _permissionItemManager.CreateAsync(
            input.ObjectName,input.Description,input.TargetType,input.RoleId,input.CanRead,input.CanCreate,input.CanEdit,input.CanDelete,input.IsActive
            );

            return ObjectMapper.Map<PermissionItem, PermissionItemDto>(permissionItem);
        }
        [Authorize(DataPermissionPermissions.PermissionItems.Create)]
        public virtual async Task<PermissionItemDto> CopyAsync(Guid id)
        {
            var permissionItem = await _permissionItemRepository.GetAsync(id);
            var newPermissionItem = await _permissionItemManager.CreateAsync(
                permissionItem.ObjectName,
                $"{permissionItem.Description}_{permissionItem.ObjectName}_Copy",
                permissionItem.TargetType,
                permissionItem.RoleId,
                permissionItem.CanRead,
                permissionItem.CanCreate,
                permissionItem.CanEdit,
                permissionItem.CanDelete,
                false
            );
            return ObjectMapper.Map<PermissionItem, PermissionItemDto>(permissionItem);
        }

        [Authorize(DataPermissionPermissions.PermissionItems.Edit)]
        public virtual async Task<PermissionItemDto> UpdateAsync(Guid id, PermissionItemUpdateDto input)
        {

            var permissionItem = await _permissionItemManager.UpdateAsync(
            id,
             input.ObjectName, input.Description, input.TargetType,input.RoleId, input.CanRead, input.CanCreate, input.CanEdit, input.CanDelete,input.IsActive, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<PermissionItem, PermissionItemDto>(permissionItem);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(PermissionItemExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _permissionItemRepository.GetListAsync(input.FilterText, input.ObjectName, input.Description, input.TargetType,input.RoleId, input.CanRead, input.CanCreate, input.CanEdit, input.CanDelete,input.IsActive);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<PermissionItem>, List<PermissionItemExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "PermissionItems.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new PermissionItemExcelDownloadTokenCacheItem { Token = token },
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