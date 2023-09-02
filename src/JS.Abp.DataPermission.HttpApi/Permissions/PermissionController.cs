using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionTypes;
using JS.Abp.DataPermission.Services;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace JS.Abp.DataPermission.Permissions;

[RemoteService(Name = "DataPermission")]
[Area("dataPermission")]
[ControllerName("Permission")]
[Route("api/data-permission/permissions")]
public class PermissionController: AbpController,IPermissionApplicationService
{
    private readonly IPermissionApplicationService _permissionApplicationService;
    public PermissionController(IPermissionApplicationService permissionApplicationService)
    {
        _permissionApplicationService = permissionApplicationService;
    }
    [HttpGet]

    public virtual Task<GetPermissionResultDto> GetAsync(string id, string policyName, PermissionType permissionType)
    {
        return _permissionApplicationService.GetAsync(id, policyName, permissionType);
    }
    [HttpGet]
    [Route("data-filter")]
    public virtual Task<DataPermissionResult> GetDataFilterAsync(GetDataFilterInput input)
    {
        return _permissionApplicationService.GetDataFilterAsync(input);
    }

    [HttpGet]
    [Route("data-permission-item")]
    public virtual Task<DataPermissionItemDto> GetDataPermissionItemAsync(GetPermissionItemInput input)
    {
        return _permissionApplicationService.GetDataPermissionItemAsync(input);
    }
}