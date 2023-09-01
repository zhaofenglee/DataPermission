using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace JS.Abp.DataPermission.PermissionItems
{
    public class PermissionItemDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string ObjectName { get; set; }
        public string? Description { get; set; }
        public string TargetType { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool IsActive { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}