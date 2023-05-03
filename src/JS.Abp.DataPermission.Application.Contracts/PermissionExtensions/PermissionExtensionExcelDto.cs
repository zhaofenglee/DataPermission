using JS.Abp.DataPermission.PermissionTypes;
using System;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public class PermissionExtensionExcelDto
    {
        public string ObjectName { get; set; }
        public Guid RoleId { get; set; }
        public Guid? ExcludedRoleId { get; set; }
        public PermissionType PermissionType { get; set; }
        public string LambdaString { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }
}