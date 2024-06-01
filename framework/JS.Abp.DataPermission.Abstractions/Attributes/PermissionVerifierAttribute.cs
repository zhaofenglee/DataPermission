using System;

namespace Attributes;
/// <summary>
/// 使用数据权限配置时需要标记此特性
/// </summary>

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class PermissionVerifierAttribute : Attribute
{
    public string ObjectName { get; set; }
    public string TargetType { get; set; }
    public PermissionVerifierAttribute(string objectName)
    {
        ObjectName = objectName;
    }
    public PermissionVerifierAttribute(string objectName,string targetType)
    {
        ObjectName = objectName;
        TargetType = targetType;
    }
}