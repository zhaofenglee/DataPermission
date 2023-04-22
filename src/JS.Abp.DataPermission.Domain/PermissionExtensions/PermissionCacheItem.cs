using System;
using Volo.Abp.Caching;

namespace JS.Abp.DataPermission.PermissionExtensions;
[CacheName("Permissions")]
public class PermissionCacheItem
{
    public string Id { get; set; }
    public string ObjectName { get; set; }
    public Guid? UserId { get; set; }
    public bool CanRead { get; set; }
    public bool CanCreate { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }
    
    
}