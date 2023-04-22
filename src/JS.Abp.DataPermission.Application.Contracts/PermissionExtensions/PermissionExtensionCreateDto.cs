using JS.Abp.DataPermission.PermissionTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public class PermissionExtensionCreateDto
    {
        [Required]
        [StringLength(PermissionExtensionConsts.ObjectNameMaxLength)]
        public string ObjectName { get; set; }
        public Guid RoleId { get; set; }
        public Guid? ExcludedRoleId { get; set; }
        public PermissionType PermissionType { get; set; } = ((PermissionType[])Enum.GetValues(typeof(PermissionType)))[0];
        [Required]
        public string LambdaString { get; set; }
        public bool IsActive { get; set; } = true;
    }
}