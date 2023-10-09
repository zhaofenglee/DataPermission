using System.Security.Principal;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Volo.Abp.SimpleStateChecking;

namespace JS.Abp.DataPermission.Authorization.Tests;

public class TestRequireEditionPermissionSimpleStateChecker : ISimpleStateChecker<PermissionDefinition>
{
    public Task<bool> IsEnabledAsync(SimpleStateCheckerContext<PermissionDefinition> context)
    {
        var currentPrincipalAccessor = context.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
        return Task.FromResult(currentPrincipalAccessor.Principal?.FindEditionId() != null);
    }
}
