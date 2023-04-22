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

namespace JS.Abp.DataPermission.Services;

public class PermissionApplicationService : ApplicationService, IPermissionApplicationService
{
    private readonly IDistributedCache<PermissionCacheItem, PermissionCacheKey> _cache;
    
    public PermissionApplicationService(IDistributedCache<PermissionCacheItem,PermissionCacheKey>  cache)
    {
        _cache = cache;
    }
    public async Task<GetPermissionResultDto> GetAsync(string id, string policyName, PermissionType permissionType)
    {
        var result = await AuthorizationService.IsGrantedAnyAsync(policyName);
        if (result == false)
        {
            return new GetPermissionResultDto()
            {
                IsGranted = false
            };
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
}