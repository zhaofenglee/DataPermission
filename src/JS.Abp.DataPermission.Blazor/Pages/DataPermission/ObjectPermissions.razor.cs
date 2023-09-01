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
using JS.Abp.DataPermission.ObjectPermissions;
using JS.Abp.DataPermission.Permissions;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.Blazor.Pages.DataPermission
{
    public partial class ObjectPermissions
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<ObjectPermissionDto> ObjectPermissionList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateObjectPermission { get; set; }
        private bool CanEditObjectPermission { get; set; }
        private bool CanDeleteObjectPermission { get; set; }
        private ObjectPermissionCreateDto NewObjectPermission { get; set; }
        private Validations NewObjectPermissionValidations { get; set; } = new();
        private ObjectPermissionUpdateDto EditingObjectPermission { get; set; }
        private Validations EditingObjectPermissionValidations { get; set; } = new();
        private Guid EditingObjectPermissionId { get; set; }
        private Modal CreateObjectPermissionModal { get; set; } = new();
        private Modal EditObjectPermissionModal { get; set; } = new();
        private GetObjectPermissionsInput Filter { get; set; }
        private DataGridEntityActionsColumn<ObjectPermissionDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "objectPermission-create-tab";
        protected string SelectedEditTab = "objectPermission-edit-tab";
        
        public ObjectPermissions()
        {
            NewObjectPermission = new ObjectPermissionCreateDto();
            EditingObjectPermission = new ObjectPermissionUpdateDto();
            Filter = new GetObjectPermissionsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            ObjectPermissionList = new List<ObjectPermissionDto>();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:ObjectPermissions"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewObjectPermission"], async () =>
            {
                await OpenCreateObjectPermissionModalAsync();
            }, IconName.Add, requiredPolicyName: DataPermissionPermissions.ObjectPermissions.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateObjectPermission = await AuthorizationService
                .IsGrantedAsync(DataPermissionPermissions.ObjectPermissions.Create);
            CanEditObjectPermission = await AuthorizationService
                            .IsGrantedAsync(DataPermissionPermissions.ObjectPermissions.Edit);
            CanDeleteObjectPermission = await AuthorizationService
                            .IsGrantedAsync(DataPermissionPermissions.ObjectPermissions.Delete);
        }

        private async Task GetObjectPermissionsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await ObjectPermissionsAppService.GetListAsync(Filter);
            ObjectPermissionList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetObjectPermissionsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await ObjectPermissionsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("DataPermission") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/data-permission/object-permissions/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ObjectPermissionDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetObjectPermissionsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateObjectPermissionModalAsync()
        {
            NewObjectPermission = new ObjectPermissionCreateDto{
                
                
            };
            await NewObjectPermissionValidations.ClearAll();
            await CreateObjectPermissionModal.Show();
        }

        private async Task CloseCreateObjectPermissionModalAsync()
        {
            NewObjectPermission = new ObjectPermissionCreateDto{
                
                
            };
            await CreateObjectPermissionModal.Hide();
        }

        private async Task OpenEditObjectPermissionModalAsync(ObjectPermissionDto input)
        {
            var objectPermission = await ObjectPermissionsAppService.GetAsync(input.Id);
            
            EditingObjectPermissionId = objectPermission.Id;
            EditingObjectPermission = ObjectMapper.Map<ObjectPermissionDto, ObjectPermissionUpdateDto>(objectPermission);
            await EditingObjectPermissionValidations.ClearAll();
            await EditObjectPermissionModal.Show();
        }
        private async Task OpenObjectPermissionDetailAsync(ObjectPermissionDto input)
        {
            var detailUrl = $"/DataPermission/ObjectPermissionDetails/{input.Id}";
            NavigationManager.NavigateTo(detailUrl);
           
        }
        private async Task DeleteObjectPermissionAsync(ObjectPermissionDto input)
        {
            await ObjectPermissionsAppService.DeleteAsync(input.Id);
            await GetObjectPermissionsAsync();
        }

        private async Task CreateObjectPermissionAsync()
        {
            try
            {
                if (await NewObjectPermissionValidations.ValidateAll() == false)
                {
                    return;
                }

                await ObjectPermissionsAppService.CreateAsync(NewObjectPermission);
                await GetObjectPermissionsAsync();
                await CloseCreateObjectPermissionModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditObjectPermissionModalAsync()
        {
            await EditObjectPermissionModal.Hide();
        }

        private async Task UpdateObjectPermissionAsync()
        {
            try
            {
                if (await EditingObjectPermissionValidations.ValidateAll() == false)
                {
                    return;
                }

                await ObjectPermissionsAppService.UpdateAsync(EditingObjectPermissionId, EditingObjectPermission);
                await GetObjectPermissionsAsync();
                await EditObjectPermissionModal.Hide();                
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
