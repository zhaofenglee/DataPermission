using System.Threading.Tasks;
using JetBrains.Annotations;
using JS.Abp.DataPermission.PermissionTypes;
using Volo.Abp.Application.Services;

namespace JS.Abp.DataPermission.Services;

public interface IPermissionApplicationService : IApplicationService
{
    Task<GetPermissionResultDto> GetAsync([NotNull]string id,[NotNull]string policyName,[NotNull] PermissionType permissionType);
}