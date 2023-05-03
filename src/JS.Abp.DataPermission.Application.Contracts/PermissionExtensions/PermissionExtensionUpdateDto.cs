using JS.Abp.DataPermission.PermissionTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public class PermissionExtensionUpdateDto : IHasConcurrencyStamp
    {
        [Required]
        [StringLength(PermissionExtensionConsts.ObjectNameMaxLength)]
        public string ObjectName { get; set; }
        public Guid RoleId { get; set; }
        public Guid? ExcludedRoleId { get; set; }
        public PermissionType PermissionType { get; set; }
        [Required]
        public string LambdaString { get; set; }
        public bool IsActive { get; set; }

        public string ConcurrencyStamp { get; set; }
        [StringLength(PermissionExtensionConsts.DescriptionMaxLength)]
        public string? Description { get; set; }
    }
}