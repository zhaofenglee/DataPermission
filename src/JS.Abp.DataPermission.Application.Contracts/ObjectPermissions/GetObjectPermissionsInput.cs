using Volo.Abp.Application.Dtos;
using System;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public class GetObjectPermissionsInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public string? ObjectName { get; set; }
        public string? Description { get; set; }

        public GetObjectPermissionsInput()
        {

        }
    }
}