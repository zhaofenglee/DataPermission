using JS.Abp.DataPermission.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace JS.Abp.DataPermission.Pages;

public abstract class DataPermissionPageModel : AbpPageModel
{
    protected DataPermissionPageModel()
    {
        LocalizationResourceType = typeof(DataPermissionResource);
    }
}
