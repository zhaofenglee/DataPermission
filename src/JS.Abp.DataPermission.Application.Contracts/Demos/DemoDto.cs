using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace JS.Abp.DataPermission.Demos
{
    public class DemoDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? Name { get; set; }
        public string? DisplayName { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}