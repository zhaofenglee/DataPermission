using System.Collections.Generic;

namespace JS.Abp.DataPermission.Services;

public class DataPermissionItemDto
{
    public string? ObjectName { get; set; }
    public List<PermissionItemResult> PermissionItems { get; set; } 
    
    public DataPermissionItemDto()
    {
        PermissionItems = new List<PermissionItemResult>();
    }
}