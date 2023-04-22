using JS.Abp.DataPermission.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JS.Abp.DataPermission.PermissionExtensions;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.PermissionExtensions
{
    public class CreateModalModel : DataPermissionPageModel
    {
        [BindProperty]
        public PermissionExtensionCreateViewModel PermissionExtension { get; set; }

        private readonly IPermissionExtensionsAppService _permissionExtensionsAppService;

        public CreateModalModel(IPermissionExtensionsAppService permissionExtensionsAppService)
        {
            _permissionExtensionsAppService = permissionExtensionsAppService;

            PermissionExtension = new();
        }

        public async Task OnGetAsync()
        {
            PermissionExtension = new PermissionExtensionCreateViewModel();

            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await _permissionExtensionsAppService.CreateAsync(ObjectMapper.Map<PermissionExtensionCreateViewModel, PermissionExtensionCreateDto>(PermissionExtension));
            return NoContent();
        }
    }

    public class PermissionExtensionCreateViewModel : PermissionExtensionCreateDto
    {
    }
}