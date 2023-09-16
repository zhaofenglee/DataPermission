using System.Collections.Generic;
using System.Threading.Tasks;
using JS.Abp.DataPermission.ObjectPermissions;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.Components.PermissionExtensions;

[Widget(
    StyleFiles = new[]
    {
        "/Pages/DataPermission/Components/PermissionExtensions/default.css"
    },
    ScriptFiles = new[]
    {
        "/Pages/DataPermission/Components/PermissionExtensions/default.js"
    })]
public class PermissionExtensionsViewComponent : AbpViewComponent
{
    
    
    private readonly IPermissionExtensionsAppService _permissionExtensionsAppService;
    
    public PermissionExtensionsViewComponent(IPermissionExtensionsAppService permissionExtensionsAppService)
    {
        _permissionExtensionsAppService = permissionExtensionsAppService;
    }
    
    public virtual async Task<IViewComponentResult> InvokeAsync(string objectName)
    {
        
        return View(
            "~/Pages/DataPermission/Components/PermissionExtensions/Default.cshtml",
            new PermissionExtensionsViewModel
            {
                ObjectNameFilter = objectName,
            });
    }

    
}

public class PermissionExtensionsViewModel
{
    public string? ObjectNameFilter { get; set; }
    public string? RoleIdFilter { get; set; }
    public string? RoleNameFilter { get; set; }
    public string? ExcludedRoleIdFilter { get; set; }
    public PermissionType? PermissionTypeFilter { get; set; }
    public string? LambdaStringFilter { get; set; }
    [SelectItems(nameof(IsActiveBoolFilterItems))]
    public string IsActiveFilter { get; set; }
    public string? DescriptionFilter { get; set; }
    public List<SelectListItem> IsActiveBoolFilterItems { get; set; } =
        new List<SelectListItem>
        {
            new SelectListItem("", ""),
            new SelectListItem("Yes", "true"),
            new SelectListItem("No", "false"),
        };
}