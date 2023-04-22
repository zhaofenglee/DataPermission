using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using JS.Abp.DataPermission.Demos;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.Demos
{
    public class IndexModel : AbpPageModel
    {
        public string? NameFilter { get; set; }
        public string? DisplayNameFilter { get; set; }

        private readonly IDemosAppService _demosAppService;

        public IndexModel(IDemosAppService demosAppService)
        {
            _demosAppService = demosAppService;
        }

        public async Task OnGetAsync()
        {

            await Task.CompletedTask;
        }
    }
}