using System;

namespace JS.Abp.DataPermission.PermissionExtensions;

public class PermissionCacheKey
{
    public Guid UserId { get; set; }
 
    public string? EntityId { get; set; }

    //构建缓存key
    public override string ToString()
    {
        return $"{UserId.ToString().ToLower()}_{EntityId?.ToLower()}";
    }
}