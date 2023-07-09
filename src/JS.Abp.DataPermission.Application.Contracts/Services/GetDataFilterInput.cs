using System;
using JS.Abp.DataPermission.PermissionTypes;

namespace JS.Abp.DataPermission.Services;

public class GetDataFilterInput
{
    public string? ObjectName { get; set; }
    public PermissionType? PermissionType { get; set; }
}