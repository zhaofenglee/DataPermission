using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public class ObjectPermissionDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string ObjectName { get; set; }
        public string? Description { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}