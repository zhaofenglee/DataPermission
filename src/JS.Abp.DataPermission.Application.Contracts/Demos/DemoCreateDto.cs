using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace JS.Abp.DataPermission.Demos
{
    public class DemoCreateDto
    {
        [StringLength(DemoConsts.NameMaxLength)]
        public string? Name { get; set; }
        [StringLength(DemoConsts.DisplayNameMaxLength)]
        public string? DisplayName { get; set; }
    }
}