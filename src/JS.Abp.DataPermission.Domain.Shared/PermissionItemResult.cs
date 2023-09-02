using System;

namespace JS.Abp.DataPermission;

public class PermissionItemResult
{
    public string ObjectName { get; set; }
    public string TargetType { get; set; }
    public Guid UserId { get; set; }
    public bool CanRead { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool IsActive { get; set; }
}