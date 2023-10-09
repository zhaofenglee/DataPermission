using System;

namespace System.ComponentModel.DataAnnotations;

public class PermissionAttribute : Attribute
{
    public string PermissionName { get; set; }

    public PermissionAttribute(string permissionName)
    {
        PermissionName = permissionName;
    }
   
}