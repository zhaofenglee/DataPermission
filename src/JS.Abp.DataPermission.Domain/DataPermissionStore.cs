using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionTypes;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
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
    protected DataPermissionOptions Options { get; }
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly IOrganizationStore _organizationStore;
    public DataPermissionStore(ICurrentUser currentUser, 
        IRepository<PermissionExtension, Guid> permissionExtensionRepository,
        IDistributedCache<List<DataPermissionResult>> cache,
        IDistributedCache<PermissionCacheItem,PermissionCacheKey> cacheItem,
        IOptions<DataPermissionOptions> options,
        IIdentityUserRepository identityUserRepository,
        IOrganizationStore organizationStore
    )
    {
        _currentUser = currentUser;
        PermissionExtensionRepository = permissionExtensionRepository;
        _cache = cache;
        _cacheItem = cacheItem;
        Options = options.Value;
        _identityUserRepository = identityUserRepository;
        _organizationStore = organizationStore;
    }

    public virtual IQueryable<TEntity> EntityFilter<TEntity>(IQueryable<TEntity> query)
    {
        if (_currentUser?.Id == null )
        { 
            return query;
        }
        else
        {
            return DataPermissionExtensions.EntityFilter(query, GetAll().Where(c=>c.UserId==(Guid )_currentUser.Id).ToList());

        }
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
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Options.CacheExpirationTime)
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

        var permissions = (await GetAllAsync()).Where(c=>c.UserId==_currentUser?.Id&&c.ObjectName==typeof(T).Name&&c.PermissionType!=PermissionType.Read).ToList();
        if (permissions.Any())
        {
            if (cacheItem==null)
            {
                cacheItem = new PermissionCacheItem()
                {
                    Id = id,
                    ObjectName = typeof(T).Name,
                    UserId = _currentUser?.Id,
                    CanRead = true,
                };
            }
            
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
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Options.CacheExpirationTime)
            });
        }
        else
        {
            if (cacheItem!=null)
            {
               await _cacheItem.RemoveAsync(new PermissionCacheKey()
               {
                   EntityId = id,
                   UserId = _currentUser?.Id??Guid.Empty
               });
            }
        }
        
       
        return true;
    }

    public virtual  PermissionCacheItem GetPermissionById<T>(string id, T item)
    {
        return AsyncHelper.RunSync(()=>GetPermissionByIdAsync(id,item));
    }

    public virtual async Task<PermissionCacheItem> GetPermissionByIdAsync<T>(string id, T item)
    {
       PermissionCacheItem cacheItem = await _cacheItem.GetAsync(new PermissionCacheKey()
        {
            EntityId = id,
            UserId = _currentUser?.Id ?? Guid.Empty
        });

        var permissions = (await GetAllAsync()).Where(c =>
                c.UserId == _currentUser?.Id && c.ObjectName == typeof(T).Name &&
                c.PermissionType != PermissionType.Read)
            .ToList();
        if (permissions.Any())
        {
            if (cacheItem == null)
            {
                cacheItem = new PermissionCacheItem()
                {
                    Id = id,
                    ObjectName = typeof(T).Name,
                    UserId = _currentUser?.Id
                };
            }

            foreach (var permission in permissions)
            {
                switch (permission.PermissionType)
                {
                    case PermissionType.UpdateAndDelete:
                        var canUpdateAndDelete =
                            DataPermissionExtensions.CheckItem(item, permission, PermissionType.UpdateAndDelete);
                        cacheItem.CanUpdate = canUpdateAndDelete;
                        cacheItem.CanDelete = canUpdateAndDelete;
                        break;
                    case PermissionType.Create:
                        var canCreate = DataPermissionExtensions.CheckItem(item, permission, PermissionType.Create);
                        cacheItem.CanCreate = canCreate;
                        break;
                    case PermissionType.Update:
                        var canUpdate = DataPermissionExtensions.CheckItem(item, permission, PermissionType.Update);
                        cacheItem.CanUpdate = canUpdate;
                        break;
                    case PermissionType.Delete:
                        var canDelete = DataPermissionExtensions.CheckItem(item, permission, PermissionType.Delete);
                        cacheItem.CanDelete = canDelete;
                        break;
                }
            }

            await _cacheItem.SetAsync(new PermissionCacheKey()
            {
                EntityId = id,
                UserId = _currentUser?.Id ?? Guid.Empty
            }, cacheItem, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Options.CacheExpirationTime)
            });
            return cacheItem;
        }
        else
        {
            if (cacheItem != null)
            {
                await _cacheItem.RemoveAsync(new PermissionCacheKey()
                {
                    EntityId = id,
                    UserId = _currentUser?.Id ?? Guid.Empty
                });
            }
            return  new PermissionCacheItem()
            {
                Id = id,
                ObjectName = typeof(T).Name,
                UserId = _currentUser?.Id,
                CanRead = true,
                CanCreate = true,
                CanUpdate = true,
                CanDelete = true,
            };
        }
    }

    public virtual PermissionCacheItem GetPermission<TEntity>(TEntity entity)
    {
        return AsyncHelper.RunSync(()=>GetPermissionAsync(entity));
    }

    public virtual async Task<PermissionCacheItem> GetPermissionAsync<TEntity>(TEntity entity)
    {
        string id = entity.GetType().GetProperty("Id")?.GetValue(entity)?.ToString();
        return  await GetPermissionByIdAsync(id, entity);
    }

    public virtual bool CheckPermission<T>(T item, PermissionType permissionType)
    {
        var result= AsyncHelper.RunSync(()=>{   return  CheckPermissionAsync(item,permissionType);   });
        return result;
    }

    public async Task<bool> CheckPermissionAsync<T>(T item, PermissionType permissionType)
    {
        var permissions = (await GetAllAsync()).Where(c =>
                c.UserId == _currentUser?.Id && c.ObjectName == typeof(T).Name && c.PermissionType == permissionType)
            .ToList();
        if (permissions.Any())
        {
            return DataPermissionExtensions.CheckItem(item, permissions.FirstOrDefault()!,
                permissionType);
        }
        else
        {
            return true;
        }
    }

    private async Task<List<DataPermissionResult>> GetFromDatabaseAsync()
    {
        var permissionList = await PermissionExtensionRepository.GetListAsync(c => c.IsActive);
        if (permissionList.Any())
        {
            var result = new List<DataPermissionResult>();
            foreach (var item in permissionList)
            {
                var userLists = await _identityUserRepository.GetListAsync(roleId: item.RoleId,notActive:false);
                //获取所有用户角色
                foreach (var user in userLists)
                {
                    //如果包含CurrentUser 要替换成当前用户
                    string lambdaString =  item.LambdaString.Replace("CurrentUser", user.Id.ToString());
                    if (item.LambdaString.Contains("OrganizationUser"))
                    {
                        //如果包含OrganizationUser要按整个组织设置查询权限
                        string newLambdaString = "";
                        var members = await _organizationStore.GetMenberInOrganizationUnitAsync(user.Id);
                        if (members.Any())
                        {
                            foreach (var menber in members)
                            {
                                if (newLambdaString == "")
                                {
                                    newLambdaString =$"({lambdaString.Replace("OrganizationUser", menber)})";
                                }
                                else
                                {
                                    newLambdaString +=$"||({lambdaString.Replace("OrganizationUser", menber)})";
                                }
                                       
                            }
                        }
                        if (newLambdaString != "")
                        {
                            lambdaString = $"({newLambdaString})";
                        }
                    }
                    result.Add(new DataPermissionResult()
                    {
                        PermissionType = item.PermissionType,
                        ObjectName = item.ObjectName,
                        LambdaString = lambdaString,
                        UserId = user.Id
                    });
                }
              
            }

            return result;
            // var result = new List<DataPermissionResult>();
            // var userLists = await _identityUserRepository.GetListAsync();
            // //获取所有用户角色
            // foreach (var user in userLists)
            // {
            //     var roles = await _identityUserRepository.GetRolesAsync(user.Id);
            //     if (roles.Any())
            //     {
            //         //查找角色权限
            //         foreach (var role in roles)
            //         {
            //             foreach (var item in permissionList.Where(c=>c.IsActive&&c.RoleId==role.Id))
            //             {
            //                 result.Add(new DataPermissionResult()
            //                 {
            //                     PermissionType = item.PermissionType,
            //                     ObjectName = item.ObjectName,
            //                     LambdaString = item.LambdaString,
            //                     UserId = user.Id
            //                 });
            //             }
            //         }
            //     }
            // }
            //
            //
            // return result;
            
        }
        else
        {
            return new List<DataPermissionResult>();
        }
    }

}
