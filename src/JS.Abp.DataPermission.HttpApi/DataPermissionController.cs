using JS.Abp.DataPermission.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace JS.Abp.DataPermission;

public abstract class DataPermissionController : AbpControllerBase
{
    protected DataPermissionController()
    {
        LocalizationResource = typeof(DataPermissionResource);
    }
}
