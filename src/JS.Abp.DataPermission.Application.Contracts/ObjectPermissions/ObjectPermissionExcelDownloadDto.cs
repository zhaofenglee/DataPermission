using Volo.Abp.Application.Dtos;
using System;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    public class ObjectPermissionExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string? FilterText { get; set; }

        public string? ObjectName { get; set; }
        public string? Description { get; set; }

        public ObjectPermissionExcelDownloadDto()
        {

        }
    }
}