using System.Collections.Generic;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionExtensions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace JS.Abp.DataPermission.Distributed;

public class MyDistributedIdentityUserEventHandler:
    IDistributedEventHandler<EntityCreatedEto<UserEto>>,
    IDistributedEventHandler<EntityUpdatedEto<UserEto>>,
    ITransientDependency
{
    private readonly IDistributedCache<List<DataPermissionResult>> _cache;
    private readonly IDistributedCache<PermissionCacheItem,PermissionCacheKey> _cacheItem;
    
    public MyDistributedIdentityUserEventHandler(IDistributedCache<List<DataPermissionResult>> cache,
        IDistributedCache<PermissionCacheItem,PermissionCacheKey> cacheItem)
    {
        _cache = cache;
        _cacheItem = cacheItem;
    }
    
    public virtual async Task HandleEventAsync(EntityCreatedEto<UserEto> eventData)
    {
        await _cache.RemoveAsync(
            DataPermissionConts.DataPermissionCacheKey
        );
    }
    
    public virtual async Task HandleEventAsync(EntityUpdatedEto<UserEto> eventData)
    {
        await _cache.RemoveAsync(
            DataPermissionConts.DataPermissionCacheKey
        );
    }

}