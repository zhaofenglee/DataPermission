using JS.Abp.DataPermission.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionExtensions;
using Microsoft.AspNetCore.Mvc;
using JS.Abp.DataPermission.PermissionItems;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.PermissionItems
{
    public class CreateModalModel : DataPermissionPageModel
    {
        [BindProperty]
        public PermissionItemCreateViewModel PermissionItem { get; set; }

        private readonly IPermissionItemsAppService _permissionItemsAppService;
        public List<SelectListItem> RoleList { get; set; }
        private readonly IPermissionExtensionsAppService _permissionExtensionsAppService;
        public CreateModalModel(IPermissionItemsAppService permissionItemsAppService,
            IPermissionExtensionsAppService permissionExtensionsAppService)
        {
            _permissionItemsAppService = permissionItemsAppService;
_permissionExtensionsAppService = permissionExtensionsAppService;
            PermissionItem = new();
        }

        public async Task OnGetAsync()
        {
            PermissionItem = new PermissionItemCreateViewModel();
            RoleList = (await _permissionExtensionsAppService.GetPermissionRoleListAsync())
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await _permissionItemsAppService.CreateAsync(ObjectMapper.Map<PermissionItemCreateViewModel, PermissionItemCreateDto>(PermissionItem));
            return NoContent();
        }
    }

    public class PermissionItemCreateViewModel : PermissionItemCreateDto
    {
    }
}