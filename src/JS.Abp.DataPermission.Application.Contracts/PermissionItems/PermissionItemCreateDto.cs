using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace JS.Abp.DataPermission.PermissionItems
{
    public class PermissionItemCreateDto
    {
        [Required]
        [StringLength(PermissionItemConsts.ObjectNameMaxLength)]
        public string ObjectName { get; set; }
        public string? Description { get; set; }
        [Required]
        [StringLength(PermissionItemConsts.TargetTypeMaxLength)]
        public string TargetType { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool IsActive { get; set; }
    }
}