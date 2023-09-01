using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public class ObjectPermissionUpdateDto : IHasConcurrencyStamp
    {
        [Required]
        [StringLength(ObjectPermissionConsts.ObjectNameMaxLength)]
        public string ObjectName { get; set; }
        public string? Description { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}