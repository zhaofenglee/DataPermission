using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using JS.Abp.DataPermission.Demos;
using JS.Abp.DataPermission.Permissions;
using JS.Abp.DataPermission.PermissionTypes;
using JS.Abp.DataPermission.Services;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.Blazor.Pages.DataPermission
{
    public partial class Demos
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<DemoDto> DemoList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateDemo { get; set; }
        private bool CanEditDemo { get; set; }
        private bool CanDeleteDemo { get; set; }
        private DemoCreateDto NewDemo { get; set; }
        private Validations NewDemoValidations { get; set; } = new();
        private DemoUpdateDto EditingDemo { get; set; }
        private Validations EditingDemoValidations { get; set; } = new();
        private Guid EditingDemoId { get; set; }
        private Modal CreateDemoModal { get; set; } = new();
        private Modal EditDemoModal { get; set; } = new();
        private GetDemosInput Filter { get; set; }
        private DataGridEntityActionsColumn<DemoDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "demo-create-tab";
        protected string SelectedEditTab = "demo-edit-tab";
        private DataPermissionItemDto DataPermissionItem { get; set; }
        public Demos()
        {
            NewDemo = new DemoCreateDto();
            EditingDemo = new DemoUpdateDto();
            Filter = new GetDemosInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            DemoList = new List<DemoDto>();
            DataPermissionItem = new DataPermissionItemDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Demos"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewDemo"], async () =>
            {
                await OpenCreateDemoModalAsync();
            }, IconName.Add, requiredPolicyName: DataPermissionPermissions.Demos.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateDemo = await AuthorizationService
                .IsGrantedAsync(DataPermissionPermissions.Demos.Create);
            // CanEditDemo = await AuthorizationService
            //     .IsGrantedAsync(DataPermissionPermissions.Demos.Edit);
            CanDeleteDemo = await AuthorizationService
                            .IsGrantedAsync(DataPermissionPermissions.Demos.Delete);
            //这里把字段级权限获取，再传递给前端，前端根据权限判断是否显示编辑
            //需要注意判断是否有权限规则如下参考：
            //if (!DataPermissionItem.PermissionItems.Any(x => x.TargetType == "DisplayName")||DataPermissionItem.PermissionItems.FirstOrDefault(x => x.TargetType == "DisplayName").CanEdit)
            DataPermissionItem =
                await PermissionApplicationService.GetDataPermissionItemAsync(new GetPermissionItemInput()
                {
                    ObjectName = "Demo",
                });
        }

        private async Task GetDemosAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await DemosAppService.GetListAsync(Filter);
            DemoList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetDemosAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await DemosAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("DataPermission") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/data-permission/demos/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<DemoDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetDemosAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateDemoModalAsync()
        {
            NewDemo = new DemoCreateDto{
                
                
            };
            await NewDemoValidations.ClearAll();
            await CreateDemoModal.Show();
        }

        private async Task CloseCreateDemoModalAsync()
        {
            NewDemo = new DemoCreateDto{
                
                
            };
            await CreateDemoModal.Hide();
        }

        private async Task OpenEditDemoModalAsync(DemoDto input)
        {
            var demo = await DemosAppService.GetAsync(input.Id);
            //CanEditDemo =  PermissionApplicationService.GetAsync(demo.Id.ToString(),DataPermissionPermissions.Demos.Edit, PermissionType.Update).Result.IsGranted;//add
            CanEditDemo =  (await PermissionApplicationService.GetAsync(demo.Id.ToString(),null,PermissionType.Update)).IsGranted;//add
            EditingDemoId = demo.Id;
            EditingDemo = ObjectMapper.Map<DemoDto, DemoUpdateDto>(demo);
            await EditingDemoValidations.ClearAll();
            await EditDemoModal.Show();
        }

        private async Task DeleteDemoAsync(DemoDto input)
        {
            await DemosAppService.DeleteAsync(input.Id);
            await GetDemosAsync();
        }

        private async Task CreateDemoAsync()
        {
            try
            {
                if (await NewDemoValidations.ValidateAll() == false)
                {
                    return;
                }

                await DemosAppService.CreateAsync(NewDemo);
                await GetDemosAsync();
                await CloseCreateDemoModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditDemoModalAsync()
        {
            await EditDemoModal.Hide();
        }

        private async Task UpdateDemoAsync()
        {
            try
            {
                if (await EditingDemoValidations.ValidateAll() == false)
                {
                    return;
                }

                await DemosAppService.UpdateAsync(EditingDemoId, EditingDemo);
                await GetDemosAsync();
                await EditDemoModal.Hide();                
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }
        

    }
}
