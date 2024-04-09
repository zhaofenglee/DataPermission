using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using JS.Abp.DataPermission.ObjectPermissions;
using Volo.Abp.Content;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.ObjectPermissions
{
    [RemoteService(Name = "DataPermission")]
    [Area("dataPermission")]
    [ControllerName("ObjectPermission")]
    [Route("api/data-permission/object-permissions")]
    public class ObjectPermissionController : AbpController, IObjectPermissionsAppService
    {
        private readonly IObjectPermissionsAppService _objectPermissionsAppService;

        public ObjectPermissionController(IObjectPermissionsAppService objectPermissionsAppService)
        {
            _objectPermissionsAppService = objectPermissionsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<ObjectPermissionDto>> GetListAsync(GetObjectPermissionsInput input)
        {
            return _objectPermissionsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<ObjectPermissionDto> GetAsync(Guid id)
        {
            return _objectPermissionsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<ObjectPermissionDto> CreateAsync(ObjectPermissionCreateDto input)
        {
            return _objectPermissionsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<ObjectPermissionDto> UpdateAsync(Guid id, ObjectPermissionUpdateDto input)
        {
            return _objectPermissionsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _objectPermissionsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(ObjectPermissionExcelDownloadDto input)
        {
            return _objectPermissionsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _objectPermissionsAppService.GetDownloadTokenAsync();
        }
    }
}