using System;
using JS.Abp.DataPermission.Attributes;
using JS.Abp.DataPermission.Shared;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace JS.Abp.DataPermission.Demos
{
    public class DemoDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? Name { get; set; }
        [PermissionVerifier("Demo", "DisplayName")]
        public string? DisplayName { get; set; }
        public RowPermissionItemDto Permission { get; set; } = new RowPermissionItemDto();

        public string ConcurrencyStamp { get; set; }
    }
}