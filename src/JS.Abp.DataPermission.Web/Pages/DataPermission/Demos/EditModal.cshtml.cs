using JS.Abp.DataPermission.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using JS.Abp.DataPermission.Demos;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.Demos
{
    public class EditModalModel : DataPermissionPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public DemoUpdateViewModel Demo { get; set; }

        private readonly IDemosAppService _demosAppService;

        public EditModalModel(IDemosAppService demosAppService)
        {
            _demosAppService = demosAppService;

            Demo = new();
        }

        public async Task OnGetAsync()
        {
            var demo = await _demosAppService.GetAsync(Id);
            Demo = ObjectMapper.Map<DemoDto, DemoUpdateViewModel>(demo);

        }

        public async Task<NoContentResult> OnPostAsync()
        {

            await _demosAppService.UpdateAsync(Id, ObjectMapper.Map<DemoUpdateViewModel, DemoUpdateDto>(Demo));
            return NoContent();
        }
    }

    public class DemoUpdateViewModel : DemoUpdateDto
    {
    }
}