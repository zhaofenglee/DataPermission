using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using JS.Abp.DataPermission.ObjectPermissions;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.ObjectPermissions
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