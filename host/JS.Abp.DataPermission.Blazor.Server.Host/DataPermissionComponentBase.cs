using JS.Abp.DataPermission.Localization;
using Volo.Abp.AspNetCore.Components;

namespace JS.Abp.DataPermission.Blazor.Server.Host;

public abstract class DataPermissionComponentBase : AbpComponentBase
{
    protected DataPermissionComponentBase()
    {
        LocalizationResource = typeof(DataPermissionResource);
    }
}
