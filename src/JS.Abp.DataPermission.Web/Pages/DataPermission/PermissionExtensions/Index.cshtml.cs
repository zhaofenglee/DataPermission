using JS.Abp.DataPermission.PermissionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.PermissionExtensions
{
    public class IndexModel : AbpPageModel
    {
        public string? ObjectNameFilter { get; set; }
        public string? RoleIdFilter { get; set; }
        public string? ExcludedRoleIdFilter { get; set; }
        public PermissionType? PermissionTypeFilter { get; set; }
        public string? LambdaStringFilter { get; set; }
        [SelectItems(nameof(IsActiveBoolFilterItems))]
        public string IsActiveFilter { get; set; }

        public List<SelectListItem> IsActiveBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };

        private readonly IPermissionExtensionsAppService _permissionExtensionsAppService;

        public IndexModel(IPermissionExtensionsAppService permissionExtensionsAppService)
        {
            _permissionExtensionsAppService = permissionExtensionsAppService;
        }

        public async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}