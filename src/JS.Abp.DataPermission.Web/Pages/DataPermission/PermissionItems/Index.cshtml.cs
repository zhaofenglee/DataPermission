using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using JS.Abp.DataPermission.PermissionItems;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.PermissionItems
{
    public class IndexModel : AbpPageModel
    {
        public string? ObjectNameFilter { get; set; }
        public string? DescriptionFilter { get; set; }
        public string? TargetTypeFilter { get; set; }
        [SelectItems(nameof(CanReadBoolFilterItems))]
        public string CanReadFilter { get; set; }

        public List<SelectListItem> CanReadBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };
        [SelectItems(nameof(CanCreateBoolFilterItems))]
        public string CanCreateFilter { get; set; }

        public List<SelectListItem> CanCreateBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };
        [SelectItems(nameof(CanEditBoolFilterItems))]
        public string CanEditFilter { get; set; }

        public List<SelectListItem> CanEditBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };
        [SelectItems(nameof(CanDeleteBoolFilterItems))]
        public string CanDeleteFilter { get; set; }

        public List<SelectListItem> CanDeleteBoolFilterItems { get; set; } =
            new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Yes", "true"),
                new SelectListItem("No", "false"),
            };

        private readonly IPermissionItemsAppService _permissionItemsAppService;

        public IndexModel(IPermissionItemsAppService permissionItemsAppService)
        {
            _permissionItemsAppService = permissionItemsAppService;
        }

        public async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}