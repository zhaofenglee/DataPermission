using JS.Abp.DataPermission.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using JS.Abp.DataPermission.PermissionExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using JS.Abp.DataPermission.PermissionItems;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.PermissionItems
{
    public class EditModalModel : DataPermissionPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public PermissionItemUpdateViewModel PermissionItem { get; set; }
        public List<SelectListItem> RoleList { get; set; }
        private readonly IPermissionExtensionsAppService _permissionExtensionsAppService;
        private readonly IPermissionItemsAppService _permissionItemsAppService;

        public EditModalModel(IPermissionItemsAppService permissionItemsAppService,
            IPermissionExtensionsAppService permissionExtensionsAppService)
        {
            _permissionItemsAppService = permissionItemsAppService;
_permissionExtensionsAppService = permissionExtensionsAppService;
            PermissionItem = new();
        }

        public async Task OnGetAsync()
        {
            var permissionItem = await _permissionItemsAppService.GetAsync(Id);
            PermissionItem = ObjectMapper.Map<PermissionItemDto, PermissionItemUpdateViewModel>(permissionItem);
            RoleList = (await _permissionExtensionsAppService.GetPermissionRoleListAsync())
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }

        public async Task<NoContentResult> OnPostAsync()
        {

            await _permissionItemsAppService.UpdateAsync(Id, ObjectMapper.Map<PermissionItemUpdateViewModel, PermissionItemUpdateDto>(PermissionItem));
            return NoContent();
        }
    }

    public class PermissionItemUpdateViewModel : PermissionItemUpdateDto
    {
    }
}