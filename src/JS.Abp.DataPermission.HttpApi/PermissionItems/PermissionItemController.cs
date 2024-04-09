using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using JS.Abp.DataPermission.PermissionItems;
using Volo.Abp.Content;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.PermissionItems
{
    [RemoteService(Name = "DataPermission")]
    [Area("dataPermission")]
    [ControllerName("PermissionItem")]
    [Route("api/data-permission/permission-items")]
    public class PermissionItemController : AbpController, IPermissionItemsAppService
    {
        private readonly IPermissionItemsAppService _permissionItemsAppService;

        public PermissionItemController(IPermissionItemsAppService permissionItemsAppService)
        {
            _permissionItemsAppService = permissionItemsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<PermissionItemDto>> GetListAsync(GetPermissionItemsInput input)
        {
            return _permissionItemsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<PermissionItemDto> GetAsync(Guid id)
        {
            return _permissionItemsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<PermissionItemDto> CreateAsync(PermissionItemCreateDto input)
        {
            return _permissionItemsAppService.CreateAsync(input);
        }
        [HttpPost]
        [Route("copy/{id}")]
        public virtual Task<PermissionItemDto> CopyAsync(Guid id)
        {
            return _permissionItemsAppService.CopyAsync(id);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<PermissionItemDto> UpdateAsync(Guid id, PermissionItemUpdateDto input)
        {
            return _permissionItemsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _permissionItemsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(PermissionItemExcelDownloadDto input)
        {
            return _permissionItemsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _permissionItemsAppService.GetDownloadTokenAsync();
        }
    }
}