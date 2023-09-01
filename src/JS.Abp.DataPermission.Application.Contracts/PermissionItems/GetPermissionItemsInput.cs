using Volo.Abp.Application.Dtos;
using System;

namespace JS.Abp.DataPermission.PermissionItems
{
    public class GetPermissionItemsInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public string? ObjectName { get; set; }
        public string? Description { get; set; }
        public string? TargetType { get; set; }
        public Guid? RoleId { get; set; }
        public bool? CanRead { get; set; }
        public bool? CanCreate { get; set; }
        public bool? CanEdit { get; set; }
        public bool? CanDelete { get; set; }
        public bool? IsActive { get; set; }
        public GetPermissionItemsInput()
        {

        }
    }
}