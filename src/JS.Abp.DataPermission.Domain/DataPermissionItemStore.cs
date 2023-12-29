using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Attributes;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionItems;
using JS.Abp.DataPermission.PermissionTypes;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
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
    private readonly IPermissionChecker _permissionChecker;
    public DataPermissionItemStore(
        ICurrentUser currentUser,
        IDistributedCache<List<PermissionItemResult>> cache,
        IOptions<DataPermissionOptions> options,
        IIdentityUserRepository identityUserRepository,
        IRepository<PermissionItem, Guid> permissionItemRepository,
        IPermissionChecker permissionChecker)
    {
        _currentUser = currentUser;
        _cache = cache;
        Options = options.Value;
        _identityUserRepository = identityUserRepository;
        PermissionItemRepository = permissionItemRepository;
        _permissionChecker = permissionChecker;
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

    public virtual  async Task<TDto> CheckAsync<TDto>(TDto sourceDto)
    {
        var type = typeof(TDto);
        PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        DataColumnInfo columnInfo = new();
        var list = props.Select(p => 
        {
            //如果有PermissionVerifierAttribute，则优先使用PermissionVerifierAttribute
            var targetType = p.GetAttribute<PermissionVerifierAttribute>();
            if (targetType != null)
            {
                return new DataColumnInfo()
                {
                    Property = p,
                    ObjectName = targetType.ObjectName,
                    TargetType = targetType.TargetType,
                };
            }
            var targetPermission = p.GetAttribute<PermissionAttribute>();
            if (targetPermission == null)
                return null;
            return new DataColumnInfo()
            {
                Property = p,
                PermissionName = targetPermission.PermissionName,
            };

        } ).Where(_ => _ != null);
        if(list.Any())
        {
            foreach(var info in list)
            {
                var canRead = true;
                if (!info.TargetType.IsNullOrWhiteSpace())
                {
                    canRead = CheckRead(info.ObjectName, info.TargetType);
                }
                if (!info.PermissionName.IsNullOrWhiteSpace())
                {
                    canRead =  await _permissionChecker
                        .IsGrantedAsync(info.PermissionName);
                }
                if (!canRead)
                {
                    if (info.Property.PropertyType.IsGenericType && info.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        // If it's a nullable type, set the property to null
                        info.Property.SetValue(sourceDto, null);
                    }
                    else if (info.Property.PropertyType.IsValueType)
                    {
                        // If it's a value type, set the property to its default value
                        info.Property.SetValue(sourceDto, Activator.CreateInstance(info.Property.PropertyType));
                    }
                    else
                    {
                        // For reference types, you may choose to set it to null or handle differently
                        info.Property.SetValue(sourceDto, null);
                    }
                    continue;
                }
            }
        }
        return sourceDto;
    }

    public virtual async Task<List<TDto>> CheckListAsync<TDto>(IEnumerable<TDto> sourceListDto)
    {
        var listDto = sourceListDto as TDto[] ?? sourceListDto.ToArray();
        foreach (var dto in listDto)
        {
            await CheckAsync(dto);
        }
        return listDto.ToList();
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

     private class DataColumnInfo
     {
         public string ObjectName { get; set; }
         public string TargetType { get; set; }
         public string PermissionName { get; set; }

         public PropertyInfo Property { get; set; }
     }
}