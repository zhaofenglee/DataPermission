using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionTypes;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Users;

namespace JS.Abp.DataPermission.Services;

public class PermissionApplicationService : ApplicationService, IPermissionApplicationService
{
    private readonly IDistributedCache<PermissionCacheItem, PermissionCacheKey> _cache;
    private readonly IDataPermissionStore _dataPermissionStore;
    private readonly IDataPermissionItemStore _dataPermissionItemStore;
    private readonly ICurrentUser _currentUser;
    public PermissionApplicationService(IDistributedCache<PermissionCacheItem,PermissionCacheKey>  cache, 
        IDataPermissionStore dataPermissionStore,
        IDataPermissionItemStore dataPermissionItemStore,
        ICurrentUser currentUser)
    {
        _cache = cache;
        _dataPermissionStore = dataPermissionStore;
        _dataPermissionItemStore = dataPermissionItemStore;
        _currentUser = currentUser;
    }
    public virtual async Task<GetPermissionResultDto> GetAsync(string id, string? policyName, PermissionType permissionType)
    {
        if (!policyName.IsNullOrWhiteSpace())
        {
            var result = await AuthorizationService.IsGrantedAnyAsync(policyName);
            if (result == false)
            {
                return new GetPermissionResultDto()
                {
                    IsGranted = false
                };
            }
        }
        var dataPermissionResults =  await _cache.GetAsync(new PermissionCacheKey()
       {
           EntityId = id,
           UserId = CurrentUser?.Id??Guid.Empty
       });
       if (dataPermissionResults!=null)
       {
           switch (permissionType)
           {
               case PermissionType.Read:
                   if (dataPermissionResults.CanRead)
                   {
                       return new GetPermissionResultDto()
                       {
                           IsGranted = true
                       };
                   }
                   break;
               case PermissionType.Create:
                   if (dataPermissionResults.CanCreate)
                   {
                       return new GetPermissionResultDto()
                       {
                           IsGranted = true
                       };
                   }
                   break;
               case PermissionType.Delete:
                   if (dataPermissionResults.CanDelete)
                   {
                       return new GetPermissionResultDto()
                       {
                           IsGranted = true
                       };
                   }
                   break;
               case PermissionType.Update:
                   if (dataPermissionResults.CanUpdate)
                   {
                       return new GetPermissionResultDto()
                       {
                           IsGranted = true
                       };
                   }
                   break;
               case PermissionType.UpdateAndDelete:
                   if (dataPermissionResults.CanUpdate && dataPermissionResults.CanDelete)
                   {
                       return new GetPermissionResultDto()
                       {
                           IsGranted = true
                       };
                   }
                   break;
           }
           return new GetPermissionResultDto()
           {
               IsGranted = false
           };
       }
       else
       {
           return new GetPermissionResultDto()
           {
               IsGranted = true
           };
       }
        
    }

    public virtual async Task<DataPermissionResult> GetDataFilterAsync(GetDataFilterInput input)
    {
        if (_currentUser.Id == null)
        {
            return new DataPermissionResult();;
        }
        var permission = (await _dataPermissionStore.GetAllAsync()).FirstOrDefault(x => x.UserId == _currentUser.Id&&x.PermissionType==input.PermissionType&&x.ObjectName==input.ObjectName);
        if (permission == null)
        {
            return new DataPermissionResult();
        }
        else
        {
            return permission;
        }
    }

    public virtual async Task<DataPermissionItemDto> GetDataPermissionItemAsync(GetPermissionItemInput input)
    {
        var permissionItems = await _dataPermissionItemStore.GetListAsync(input.ObjectName);
        return new DataPermissionItemDto()
        {
            ObjectName = input.ObjectName,
            PermissionItems = permissionItems,
        };
    }
}