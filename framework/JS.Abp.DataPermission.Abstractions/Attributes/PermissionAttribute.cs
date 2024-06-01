using System;

namespace Attributes;
/// <summary>
/// 使用AbpPermission时需要标记此特性
/// </summary>
public class PermissionAttribute : Attribute
{
    public string PermissionName { get; set; }

    public PermissionAttribute(string permissionName)
    {
        PermissionName = permissionName;
    }
   
}