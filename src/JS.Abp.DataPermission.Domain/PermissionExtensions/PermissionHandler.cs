using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace JS.Abp.DataPermission.PermissionExtensions;

public class PermissionHandler: ILocalEventHandler<EntityChangedEventData<PermissionExtension>>,ITransientDependency
{
    private readonly IDistributedCache<List<DataPermissionResult>> _cache;
    private readonly IDistributedCache<PermissionCacheItem,PermissionCacheKey> _cacheItem;
    
    public PermissionHandler(IDistributedCache<List<DataPermissionResult>> cache,
        IDistributedCache<PermissionCacheItem,PermissionCacheKey> cacheItem)
    {
        _cache = cache;
        _cacheItem = cacheItem;
    }
    
    public async Task HandleEventAsync(EntityChangedEventData<PermissionExtension> eventData)
    {
        await _cache.RemoveAsync(
            DataPermissionConts.DataPermissionCacheKey
        );
    }
}