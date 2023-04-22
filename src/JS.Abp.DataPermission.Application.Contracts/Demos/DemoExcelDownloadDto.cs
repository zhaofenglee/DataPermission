using Volo.Abp.Application.Dtos;
using System;

namespace JS.Abp.DataPermission.Demos
{
    public class DemoExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string? FilterText { get; set; }

        public string? Name { get; set; }
        public string? DisplayName { get; set; }

        public DemoExcelDownloadDto()
        {

        }
    }
}