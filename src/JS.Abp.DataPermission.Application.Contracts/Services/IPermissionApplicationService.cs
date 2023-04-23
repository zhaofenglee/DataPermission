using System.Threading.Tasks;
using JetBrains.Annotations;
using JS.Abp.DataPermission.PermissionTypes;
using Volo.Abp.Application.Services;

namespace JS.Abp.DataPermission.Services;

public interface IPermissionApplicationService : IApplicationService
{
    Task<GetPermissionResultDto> GetAsync(string id,string policyName ="",PermissionType permissionType=PermissionType.Update);
}