using System;
using JS.Abp.DataPermission.Shared;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace JS.Abp.DataPermission.Demos
{
    public class DemoDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public PermissionItemDto Permission { get; set; } = new PermissionItemDto();

        public string ConcurrencyStamp { get; set; }
    }
}