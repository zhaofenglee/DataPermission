using JS.Abp.DataPermission.Localization;
using Volo.Abp.AspNetCore.Components;

namespace JS.Abp.DataPermission.Blazor;

public abstract class DataPermissionComponentBase : AbpComponentBase
{
    protected DataPermissionComponentBase()
    {
        LocalizationResource = typeof(DataPermissionResource);
    }
}
