using System.Threading.Tasks;
using JS.Abp.DataPermission.ObjectPermissions;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace JS.Abp.DataPermission.Web.Pages.DataPermission.ObjectPermissions
{
    public class DetailsModel : AbpPageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ObjectName { get; set; }
        public string? ObjectNameFilter { get; set; }
        public string? DescriptionFilter { get; set; }

        [BindProperty]
        public ObjectPermissionUpdateViewModel ObjectPermission { get; set; }
        
        private readonly IObjectPermissionsAppService _objectPermissionsAppService;

        public DetailsModel(IObjectPermissionsAppService objectPermissionsAppService)
        {
            _objectPermissionsAppService = objectPermissionsAppService;
            ObjectPermission = new ObjectPermissionUpdateViewModel();
        }

        public async Task OnGetAsync()
        {
            var objectPermission = await _objectPermissionsAppService.GetListAsync(new GetObjectPermissionsInput()
            {
                ObjectName = ObjectName
            });
            if (objectPermission.TotalCount > 0)
            {
                ObjectPermission = ObjectMapper.Map<ObjectPermissionDto, ObjectPermissionUpdateViewModel>(objectPermission.Items[0]);
            }
            else
            {
                //返回地址不存在
                
            }
           

            await Task.CompletedTask;
        }
    }
}