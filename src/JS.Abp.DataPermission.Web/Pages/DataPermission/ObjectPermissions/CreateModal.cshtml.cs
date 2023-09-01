using JS.Abp.DataPermission.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JS.Abp.DataPermission.ObjectPermissions;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.ObjectPermissions
{
    public class CreateModalModel : DataPermissionPageModel
    {
        [BindProperty]
        public ObjectPermissionCreateViewModel ObjectPermission { get; set; }

        private readonly IObjectPermissionsAppService _objectPermissionsAppService;

        public CreateModalModel(IObjectPermissionsAppService objectPermissionsAppService)
        {
            _objectPermissionsAppService = objectPermissionsAppService;

            ObjectPermission = new();
        }

        public async Task OnGetAsync()
        {
            ObjectPermission = new ObjectPermissionCreateViewModel();

            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await _objectPermissionsAppService.CreateAsync(ObjectMapper.Map<ObjectPermissionCreateViewModel, ObjectPermissionCreateDto>(ObjectPermission));
            return NoContent();
        }
    }

    public class ObjectPermissionCreateViewModel : ObjectPermissionCreateDto
    {
    }
}