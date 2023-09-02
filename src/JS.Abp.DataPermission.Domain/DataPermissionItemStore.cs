using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionItems;
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

public class DataPermissionItemStore:IDataPermissionItemStore,ITransientDependency
{
    private readonly ICurrentUser _currentUser;
    private readonly IDistributedCache<List<PermissionItemResult>> _cache;
    protected DataPermissionOptions Options { get; }
    private readonly IIdentityUserRepository _identityUserRepository;
    protected IRepository<PermissionItem, Guid> PermissionItemRepository { get; }
    
    public DataPermissionItemStore(
        ICurrentUser currentUser,
        IDistributedCache<List<PermissionItemResult>> cache,
        IOptions<DataPermissionOptions> options,
        IIdentityUserRepository identityUserRepository,
        IRepository<PermissionItem, Guid> permissionItemRepository)
    {
        _currentUser = currentUser;
        _cache = cache;
        Options = options.Value;
        _identityUserRepository = identityUserRepository;
        PermissionItemRepository = permissionItemRepository;
    }
    
    public virtual bool CheckPermission(string objectName, string targetName, PermissionType permissionType)
    {
        if (_currentUser.Id.HasValue)
        {
            var permissionItems = AsyncHelper.RunSync(()=>GetListAsync(objectName));
            if (!permissionItems.Any())
            {
                return true;
            }

            var permissionItem = permissionItems.Where(c =>
                c.ObjectName == objectName && c.TargetType == targetName && c.UserId == _currentUser.Id.Value)?.FirstOrDefault();
            if (permissionItem!=null)
            {
                switch (permissionType)
                {
                    case PermissionType.Create:
                        return permissionItem.CanCreate;
                    case PermissionType.Read:
                        return permissionItem.CanRead;
                    case PermissionType.Update:
                        return permissionItem.CanEdit;
                    case PermissionType.Delete:
                        return permissionItem.CanDelete;
                }
            }
            return true;
        }
        else
        {
            return true;
        }
    }

    public bool CheckCreate(string objectName, string targetName)
    {
        return CheckPermission(objectName, targetName, PermissionType.Create);
    }

    public bool CheckRead(string objectName, string targetName)
    {
       return CheckPermission(objectName, targetName, PermissionType.Read);
    }

    public bool CheckUpdate(string objectName, string targetName)
    {
        return CheckPermission(objectName, targetName, PermissionType.Update);
    }

    public bool CheckDelete(string objectName, string targetName)
    {
        return CheckPermission(objectName, targetName, PermissionType.Delete);
    }

    public virtual async Task<List<PermissionItemResult>> GetListAsync(string objectName)
    {
        if (_currentUser.Id.HasValue)
        {
            var permissionItems = (await GetAllAsync()).Where(c => c.ObjectName == objectName && c.UserId == _currentUser.Id.Value);
            return permissionItems.ToList();
        }
        else
        {
           return new List<PermissionItemResult>();
        }
      
    }
    
    private  async Task<List<PermissionItemResult>> GetAllAsync()
    {
        return  await _cache.GetOrAddAsync(
            DataPermissionConts.DataPermissionCacheItemsKey, //缓存键
            async () => await GetFromDatabaseAsync(),
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(Options.CacheExpirationTime)
            }
        )??new List<PermissionItemResult>();
    }
    
     private async Task<List<PermissionItemResult>> GetFromDatabaseAsync()
    {
        var permissionItemList = await PermissionItemRepository.GetListAsync(c => c.IsActive);
        if (permissionItemList.Any())
        {
            var result = new List<PermissionItemResult>();
            foreach (var item in permissionItemList)
            {
                var userLists = await _identityUserRepository.GetListAsync(roleId: item.RoleId,notActive:false);
                //获取所有用户角色
                foreach (var user in userLists)
                {
                    result.Add(new PermissionItemResult()
                    {
                        ObjectName = item.ObjectName,
                        TargetType = item.TargetType,
                        CanRead = item.CanRead,
                        CanCreate = item.CanCreate,
                        CanEdit = item.CanEdit,
                        CanDelete = item.CanDelete,
                        IsActive = true,
                        UserId = user.Id
                    });
                }
              
            }
            return result;
        }
        else
        {
            return new List<PermissionItemResult>();
        }
    }
}