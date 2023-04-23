using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionTypes;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Threading;
using Volo.Abp.Users;

namespace JS.Abp.DataPermission;

public class DataPermissionStore:IDataPermissionStore, ITransientDependency
{
    private readonly ICurrentUser _currentUser;
    private readonly IDistributedCache<List<DataPermissionResult>> _cache;
    private readonly IDistributedCache<PermissionCacheItem,PermissionCacheKey> _cacheItem;
    protected IRepository<PermissionExtension, Guid> PermissionExtensionRepository { get; }
    private readonly IIdentityRoleRepository IdentityRoleRepository;
    private readonly IIdentityUserRepository IdentityUserRepository;
    
    public DataPermissionStore(ICurrentUser currentUser, 
        IRepository<PermissionExtension, Guid> permissionExtensionRepository,
        IDistributedCache<List<DataPermissionResult>> cache,
        IDistributedCache<PermissionCacheItem,PermissionCacheKey> cacheItem,
        IIdentityRoleRepository identityRoleRepository,
        IIdentityUserRepository identityUserRepository
        )
    {
        _currentUser = currentUser;
        PermissionExtensionRepository = permissionExtensionRepository;
        _cache = cache;
        _cacheItem = cacheItem;
        IdentityRoleRepository = identityRoleRepository;
        IdentityUserRepository = identityUserRepository;
    }

    public virtual List<DataPermissionResult> GetAll()
    {
        var result= AsyncHelper.RunSync(()=>{   return  GetAllAsync();   });
        return result;
    }

    public virtual async Task<List<DataPermissionResult>> GetAllAsync()
    {
        return  await _cache.GetOrAddAsync(
            DataPermissionConts.DataPermissionCacheKey, //缓存键
            async () => await GetFromDatabaseAsync(),
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
            }
        );
    }

    public virtual async Task<bool> GetPermissionAsync<T>(string id, T item)
    { 
        
        PermissionCacheItem cacheItem = await  _cacheItem.GetAsync(new PermissionCacheKey()
        {
            EntityId = id,
            UserId = _currentUser?.Id??Guid.Empty
        });
        if (cacheItem==null)
        {
            cacheItem = new PermissionCacheItem()
            {
                Id = id,
                ObjectName = typeof(T).Name,
                UserId = _currentUser?.Id
            };
        }
       
        var permissions = (await GetAllAsync()).Where(c=>c.UserId==_currentUser?.Id&&c.ObjectName==typeof(T).Name&&c.PermissionType!=PermissionType.Read).ToList();
        if (permissions.Any())
        {
            foreach (var permission in permissions)
            {
                switch (permission.PermissionType)
                {
                    case PermissionType.UpdateAndDelete:
                        var canUpdateAndDelete = DataPermissionExtensions.CheckItem(item,permission,PermissionType.UpdateAndDelete);
                        cacheItem.CanUpdate = canUpdateAndDelete;
                        cacheItem.CanDelete = canUpdateAndDelete;
                        break;
                    case PermissionType.Create:
                        var canCreate = DataPermissionExtensions.CheckItem(item,permission,PermissionType.Create);
                        cacheItem.CanCreate = canCreate;
                        break;
                    case PermissionType.Update:
                        var canUpdate = DataPermissionExtensions.CheckItem(item,permission,PermissionType.Update);
                        cacheItem.CanUpdate = canUpdate;
                        break;
                    case PermissionType.Delete:
                        var canDelete = DataPermissionExtensions.CheckItem(item,permission,PermissionType.Delete);
                        cacheItem.CanDelete = canDelete;
                        break;
                }
            }
            
            await _cacheItem.SetAsync(new PermissionCacheKey()
            {
                EntityId = id,
                UserId = _currentUser?.Id??Guid.Empty
            }, cacheItem, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
            });
        }
        
       
        return true;
    }

    public async Task<bool> CheckPermissionAsync<T>(T item, PermissionType permissionType)
    {
        var permissions = (await GetAllAsync()).Where(c=>c.UserId==_currentUser?.Id&&c.ObjectName==typeof(T).Name&&c.PermissionType==permissionType).ToList();
        if (permissions.Any())
        {
            return DataPermissionExtensions.CheckItem(item,permissions.FirstOrDefault(),PermissionType.UpdateAndDelete);
        }
        else
        {
            return true;
        }
        
    }

    private async Task<List<DataPermissionResult>> GetFromDatabaseAsync()
    {
        var permissionList = await PermissionExtensionRepository.GetListAsync();
        if (permissionList.Any())
        {
            var result = new List<DataPermissionResult>();
            var userLists = await IdentityUserRepository.GetListAsync();
            //获取所有用户角色
            foreach (var user in userLists)
            {
                var roles = await IdentityUserRepository.GetRolesAsync(user.Id);
                if (roles.Any())
                {
                    //查找角色权限
                    foreach (var role in roles)
                    {
                        foreach (var item in permissionList.Where(c=>c.IsActive&&c.RoleId==role.Id))
                        {
                            result.Add(new DataPermissionResult()
                            {
                                PermissionType = item.PermissionType,
                                ObjectName = item.ObjectName,
                                LambdaString = item.LambdaString,
                                UserId = user.Id
                            });
                        }
                    }
                }
            }
           
            
            return result;
            // if (_currentUser?.Id!=null)
            // {
            //     foreach (var item in permissionList.Where(c=>c.IsActive))
            //     {
            //         result.Add(new DataPermissionResult()
            //         {
            //             PermissionType = item.PermissionType,
            //             ObjectName = item.ObjectName,
            //             LambdaString = item.LambdaString,
            //             UserId = (Guid)_currentUser.Id
            //         });
            //     }
            //     
            //     // return new List<DataPermissionResult>()
            //     // {
            //     //     new DataPermissionResult()
            //     //     {
            //     //         PermissionType = PermissionType.Read,ObjectName = "SerialNo",LambdaString = "x.Name != \"test1\" && x.Name != \"test2\" || x.Name == \"test\" && x.CreatorId==\"CurrentUser\"",UserId = (Guid)_currentUser.Id
            //     //     },
            //     //     new DataPermissionResult()
            //     //     {
            //     //         PermissionType = PermissionType.UpdateAndDelete,ObjectName = "SerialNo",LambdaString = "x.Name == \"test\" && x.CreatorId==\"CurrentUser\"",UserId = (Guid)_currentUser.Id
            //     //     },
            //     // };
            // }
            // else
            // {
            //     return new List<DataPermissionResult>();
            // }
        }
        else
        {
            return new List<DataPermissionResult>();
        }
    }

}
