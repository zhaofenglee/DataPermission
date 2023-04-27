using JS.Abp.DataPermission.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using JS.Abp.DataPermission.PermissionExtensions;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.PermissionExtensions
{
    public class EditModalModel : DataPermissionPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public PermissionExtensionUpdateViewModel PermissionExtension { get; set; }
        public List<SelectListItem> RoleList { get; set; }
        private readonly IPermissionExtensionsAppService _permissionExtensionsAppService;

        public EditModalModel(IPermissionExtensionsAppService permissionExtensionsAppService)
        {
            _permissionExtensionsAppService = permissionExtensionsAppService;

            PermissionExtension = new();
        }

        public async Task OnGetAsync()
        {
            var permissionExtension = await _permissionExtensionsAppService.GetAsync(Id);
            PermissionExtension = ObjectMapper.Map<PermissionExtensionDto, PermissionExtensionUpdateViewModel>(permissionExtension);
            RoleList = (await _permissionExtensionsAppService.GetPermissionRoleListAsync())
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }

        public async Task<NoContentResult> OnPostAsync()
        {

            await _permissionExtensionsAppService.UpdateAsync(Id, ObjectMapper.Map<PermissionExtensionUpdateViewModel, PermissionExtensionUpdateDto>(PermissionExtension));
            return NoContent();
        }
    }

    public class PermissionExtensionUpdateViewModel : PermissionExtensionUpdateDto
    {
    }
}