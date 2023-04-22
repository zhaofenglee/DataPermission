using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using JS.Abp.DataPermission.PermissionExtensions;
using Volo.Abp.Content;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.PermissionExtensions
{
    [RemoteService(Name = "DataPermission")]
    [Area("dataPermission")]
    [ControllerName("PermissionExtension")]
    [Route("api/data-permission/permission-extensions")]
    public class PermissionExtensionController : AbpController, IPermissionExtensionsAppService
    {
        private readonly IPermissionExtensionsAppService _permissionExtensionsAppService;

        public PermissionExtensionController(IPermissionExtensionsAppService permissionExtensionsAppService)
        {
            _permissionExtensionsAppService = permissionExtensionsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<PermissionExtensionDto>> GetListAsync(GetPermissionExtensionsInput input)
        {
            return _permissionExtensionsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<PermissionExtensionDto> GetAsync(Guid id)
        {
            return _permissionExtensionsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<PermissionExtensionDto> CreateAsync(PermissionExtensionCreateDto input)
        {
            return _permissionExtensionsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<PermissionExtensionDto> UpdateAsync(Guid id, PermissionExtensionUpdateDto input)
        {
            return _permissionExtensionsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _permissionExtensionsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(PermissionExtensionExcelDownloadDto input)
        {
            return _permissionExtensionsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _permissionExtensionsAppService.GetDownloadTokenAsync();
        }
        [HttpGet]
        [Route("permission-role")]
        public Task<List<PermissionRoleDto>> GetPermissionRoleListAsync()
        {
            throw new NotImplementedException();
        }
    }
}