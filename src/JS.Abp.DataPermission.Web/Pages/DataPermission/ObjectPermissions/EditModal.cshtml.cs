using JS.Abp.DataPermission.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using JS.Abp.DataPermission.ObjectPermissions;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.ObjectPermissions
{
    public class EditModalModel : DataPermissionPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public ObjectPermissionUpdateViewModel ObjectPermission { get; set; }

        private readonly IObjectPermissionsAppService _objectPermissionsAppService;

        public EditModalModel(IObjectPermissionsAppService objectPermissionsAppService)
        {
            _objectPermissionsAppService = objectPermissionsAppService;

            ObjectPermission = new();
        }

        public async Task OnGetAsync()
        {
            var objectPermission = await _objectPermissionsAppService.GetAsync(Id);
            ObjectPermission = ObjectMapper.Map<ObjectPermissionDto, ObjectPermissionUpdateViewModel>(objectPermission);

        }

        public async Task<NoContentResult> OnPostAsync()
        {

            await _objectPermissionsAppService.UpdateAsync(Id, ObjectMapper.Map<ObjectPermissionUpdateViewModel, ObjectPermissionUpdateDto>(ObjectPermission));
            return NoContent();
        }
    }

    public class ObjectPermissionUpdateViewModel : ObjectPermissionUpdateDto
    {
    }
}