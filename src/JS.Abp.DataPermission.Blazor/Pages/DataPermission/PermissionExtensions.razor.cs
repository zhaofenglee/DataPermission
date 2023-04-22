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
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.Permissions;
using JS.Abp.DataPermission.Shared;

namespace JS.Abp.DataPermission.Blazor.Pages.DataPermission
{
    public partial class PermissionExtensions
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<PermissionExtensionDto> PermissionExtensionList { get; set; }
        private IReadOnlyList<PermissionRoleDto> PermissionRoleList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreatePermissionExtension { get; set; }
        private bool CanEditPermissionExtension { get; set; }
        private bool CanDeletePermissionExtension { get; set; }
        private PermissionExtensionCreateDto NewPermissionExtension { get; set; }
        private Validations NewPermissionExtensionValidations { get; set; } = new();
        private PermissionExtensionUpdateDto EditingPermissionExtension { get; set; }
        private Validations EditingPermissionExtensionValidations { get; set; } = new();
        private Guid EditingPermissionExtensionId { get; set; }
        private Modal CreatePermissionExtensionModal { get; set; } = new();
        private Modal EditPermissionExtensionModal { get; set; } = new();
        private GetPermissionExtensionsInput Filter { get; set; }
        private DataGridEntityActionsColumn<PermissionExtensionDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "permissionExtension-create-tab";
        protected string SelectedEditTab = "permissionExtension-edit-tab";
        
        public PermissionExtensions()
        {
            NewPermissionExtension = new PermissionExtensionCreateDto();
            EditingPermissionExtension = new PermissionExtensionUpdateDto();
            Filter = new GetPermissionExtensionsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            PermissionExtensionList = new List<PermissionExtensionDto>();
            PermissionRoleList = new List<PermissionRoleDto>();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
            await GetPermissionRoleAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:PermissionExtensions"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewPermissionExtension"], async () =>
            {
                await OpenCreatePermissionExtensionModalAsync();
            }, IconName.Add, requiredPolicyName: DataPermissionPermissions.PermissionExtensions.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreatePermissionExtension = await AuthorizationService
                .IsGrantedAsync(DataPermissionPermissions.PermissionExtensions.Create);
            CanEditPermissionExtension = await AuthorizationService
                            .IsGrantedAsync(DataPermissionPermissions.PermissionExtensions.Edit);
            CanDeletePermissionExtension = await AuthorizationService
                            .IsGrantedAsync(DataPermissionPermissions.PermissionExtensions.Delete);
        }

        private async Task GetPermissionExtensionsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await PermissionExtensionsAppService.GetListAsync(Filter);
            PermissionExtensionList = result.Items;
            TotalCount = (int)result.TotalCount;
        }
        private async Task GetPermissionRoleAsync()
        {
            PermissionRoleList = await PermissionExtensionsAppService.GetPermissionRoleListAsync();
        }
        
        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetPermissionExtensionsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await PermissionExtensionsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("DataPermission") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/data-permission/permission-extensions/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<PermissionExtensionDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetPermissionExtensionsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreatePermissionExtensionModalAsync()
        {
            NewPermissionExtension = new PermissionExtensionCreateDto{
                
                
            };
            await NewPermissionExtensionValidations.ClearAll();
            await CreatePermissionExtensionModal.Show();
        }

        private async Task CloseCreatePermissionExtensionModalAsync()
        {
            NewPermissionExtension = new PermissionExtensionCreateDto{
                
                
            };
            await CreatePermissionExtensionModal.Hide();
        }

        private async Task OpenEditPermissionExtensionModalAsync(PermissionExtensionDto input)
        {
            var permissionExtension = await PermissionExtensionsAppService.GetAsync(input.Id);
            
            EditingPermissionExtensionId = permissionExtension.Id;
            EditingPermissionExtension = ObjectMapper.Map<PermissionExtensionDto, PermissionExtensionUpdateDto>(permissionExtension);
            await EditingPermissionExtensionValidations.ClearAll();
            await EditPermissionExtensionModal.Show();
        }

        private async Task DeletePermissionExtensionAsync(PermissionExtensionDto input)
        {
            await PermissionExtensionsAppService.DeleteAsync(input.Id);
            await GetPermissionExtensionsAsync();
        }

        private async Task CreatePermissionExtensionAsync()
        {
            try
            {
                if (await NewPermissionExtensionValidations.ValidateAll() == false)
                {
                    return;
                }

                await PermissionExtensionsAppService.CreateAsync(NewPermissionExtension);
                await GetPermissionExtensionsAsync();
                await CloseCreatePermissionExtensionModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditPermissionExtensionModalAsync()
        {
            await EditPermissionExtensionModal.Hide();
        }

        private async Task UpdatePermissionExtensionAsync()
        {
            try
            {
                if (await EditingPermissionExtensionValidations.ValidateAll() == false)
                {
                    return;
                }

                await PermissionExtensionsAppService.UpdateAsync(EditingPermissionExtensionId, EditingPermissionExtension);
                await GetPermissionExtensionsAsync();
                await EditPermissionExtensionModal.Hide();                
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
