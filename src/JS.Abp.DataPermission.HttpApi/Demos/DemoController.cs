using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using JS.Abp.DataPermission.Demos;
using Volo.Abp.Content;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.Demos
{
    [RemoteService(Name = "DataPermission")]
    [Area("dataPermission")]
    [ControllerName("Demo")]
    [Route("api/data-permission/demos")]
    public class DemoController : AbpController, IDemosAppService
    {
        private readonly IDemosAppService _demosAppService;

        public DemoController(IDemosAppService demosAppService)
        {
            _demosAppService = demosAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<DemoDto>> GetListAsync(GetDemosInput input)
        {
            return _demosAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<DemoDto> GetAsync(Guid id)
        {
            return _demosAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<DemoDto> CreateAsync(DemoCreateDto input)
        {
            return _demosAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<DemoDto> UpdateAsync(Guid id, DemoUpdateDto input)
        {
            return _demosAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _demosAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(DemoExcelDownloadDto input)
        {
            return _demosAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _demosAppService.GetDownloadTokenAsync();
        }
    }
}