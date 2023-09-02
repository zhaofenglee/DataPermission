using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public class ObjectPermissionCreateDto
    {
        [Required]
        [StringLength(ObjectPermissionConsts.ObjectNameMaxLength)]
        public string ObjectName { get; set; }
        public string? Description { get; set; }
    }
}