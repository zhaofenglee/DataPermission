using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace JS.Abp.DataPermission.Demos
{
    public class DemoUpdateDto : IHasConcurrencyStamp
    {
        [StringLength(DemoConsts.NameMaxLength)]
        public string? Name { get; set; }
        [StringLength(DemoConsts.DisplayNameMaxLength)]
        public string? DisplayName { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}