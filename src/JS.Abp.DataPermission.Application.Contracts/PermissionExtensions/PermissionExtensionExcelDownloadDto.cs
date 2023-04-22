using JS.Abp.DataPermission.PermissionTypes;
using Volo.Abp.Application.Dtos;
using System;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    public class PermissionExtensionExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string? FilterText { get; set; }

        public string? ObjectName { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? ExcludedRoleId { get; set; }
        public PermissionType? PermissionType { get; set; }
        public string? LambdaString { get; set; }
        public bool? IsActive { get; set; }

        public PermissionExtensionExcelDownloadDto()
        {

        }
    }
}