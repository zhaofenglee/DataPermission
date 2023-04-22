using JS.Abp.DataPermission.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace JS.Abp.DataPermission.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class DataPermissionPageModel : AbpPageModel
{
    protected DataPermissionPageModel()
    {
        LocalizationResourceType = typeof(DataPermissionResource);
        ObjectMapperContext = typeof(DataPermissionWebModule);
    }
}
