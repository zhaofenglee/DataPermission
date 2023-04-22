using System;
using JS.Abp.DataPermission.PermissionTypes;

namespace JS.Abp.DataPermission;

public class DataPermissionResult
{
    public Guid UserId { get; set; }
    public string ObjectName { get; set; }
    public string LambdaString { get; set; }
    public PermissionType PermissionType { get; set; }
}