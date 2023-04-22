using Volo.Abp.Application.Dtos;
using System;

namespace JS.Abp.DataPermission.Demos
{
    public class GetDemosInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public string? Name { get; set; }
        public string? DisplayName { get; set; }

        public GetDemosInput()
        {

        }
    }
}