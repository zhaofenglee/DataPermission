using System.Collections.Generic;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.Components.PermissionItems;

[Widget(
    StyleFiles = new[]
    {
        "/Pages/DataPermission/Components/PermissionItems/default.css"
    },
    ScriptFiles = new[]
    {
        "/Pages/DataPermission/Components/PermissionItems/default.js"
    })]
public class PermissionItemsViewComponent: AbpViewComponent
{
    private readonly IPermissionItemsAppService _permissionItemsAppService;
    
    public PermissionItemsViewComponent(IPermissionItemsAppService permissionItemsAppService)
    {
        _permissionItemsAppService = permissionItemsAppService;
    }
    
    public virtual async Task<IViewComponentResult> InvokeAsync(string objectName)
    {
        var model = new PermissionItemsViewModel
        {
            ObjectNameFilter = objectName
        };
        return View(
            "~/Pages/DataPermission/Components/PermissionItems/Default.cshtml",model );
    }
    
    
}

public class PermissionItemsViewModel
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
}