using System.Threading.Tasks;
using JS.Abp.DataPermission.ObjectPermissions;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.ObjectPermissionDetails
{
    public class IndexModel : AbpPageModel
    {
        public string? ObjectNameFilter { get; set; }
        public string? DescriptionFilter { get; set; }

        private readonly IObjectPermissionsAppService _objectPermissionsAppService;

        public IndexModel(IObjectPermissionsAppService objectPermissionsAppService)
        {
            _objectPermissionsAppService = objectPermissionsAppService;
        }

        public async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}