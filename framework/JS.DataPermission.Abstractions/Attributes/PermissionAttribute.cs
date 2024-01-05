using System;

namespace System.ComponentModel.DataAnnotations;
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