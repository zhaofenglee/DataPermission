using System.Collections.Generic;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionExtensions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace JS.Abp.DataPermission.PermissionItems;

public class PermissionItemHandler: ILocalEventHandler<EntityChangedEventData<PermissionItem>>,ITransientDependency
{
    private readonly IDistributedCache<List<PermissionItemResult>> _cache;
    
    public PermissionItemHandler(IDistributedCache<List<PermissionItemResult>> cache
       )
    {
        _cache = cache;
    }
    
    public virtual async Task HandleEventAsync(EntityChangedEventData<PermissionItem> eventData)
    {
        await _cache.RemoveAsync(
            DataPermissionConts.DataPermissionCacheItemsKey
        );
    }
}