using JS.Abp.DataPermission.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JS.Abp.DataPermission.Demos;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.Demos
{
    public class CreateModalModel : DataPermissionPageModel
    {
        [BindProperty]
        public DemoCreateViewModel Demo { get; set; }

        private readonly IDemosAppService _demosAppService;

        public CreateModalModel(IDemosAppService demosAppService)
        {
            _demosAppService = demosAppService;

            Demo = new();
        }

        public async Task OnGetAsync()
        {
            Demo = new DemoCreateViewModel();

            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await _demosAppService.CreateAsync(ObjectMapper.Map<DemoCreateViewModel, DemoCreateDto>(Demo));
            return NoContent();
        }
    }

    public class DemoCreateViewModel : DemoCreateDto
    {
    }
}