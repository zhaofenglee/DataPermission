using JS.Abp.DataPermission.PermissionTypes;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public class PermissionExtensionDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string ObjectName { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public Guid? ExcludedRoleId { get; set; }
        public PermissionType PermissionType { get; set; }
        public string LambdaString { get; set; }
        public bool IsActive { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}