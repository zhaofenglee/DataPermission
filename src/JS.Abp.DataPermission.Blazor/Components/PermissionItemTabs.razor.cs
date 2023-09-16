using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using JS.Abp.DataPermission.PermissionExtensions;
using JS.Abp.DataPermission.PermissionItems;
using JS.Abp.DataPermission.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.BlazoriseUI.Components;

namespace JS.Abp.DataPermission.Blazor.Components
{
    public partial class PermissionItemTabs
    {
        [Parameter] public string ObjectName { get; set; } = string.Empty;

        private IReadOnlyList<PermissionItemDto> PermissionItemList { get; set; }
        private IReadOnlyList<PermissionRoleDto> PermissionRoleList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreatePermissionItem { get; set; }
        private bool CanEditPermissionItem { get; set; }
        private bool CanDeletePermissionItem { get; set; }
        private PermissionItemCreateDto NewPermissionItem { get; set; }
        private Validations NewPermissionItemValidations { get; set; } = new();
        private PermissionItemUpdateDto EditingPermissionItem { get; set; }
        private Validations EditingPermissionItemValidations { get; set; } = new();
        private Guid EditingPermissionItemId { get; set; }
        private Modal CreatePermissionItemModal { get; set; } = new();
        private Modal EditPermissionItemModal { get; set; } = new();
        private GetPermissionItemsInput Filter { get; set; }
        private DataGridEntityActionsColumn<PermissionItemDto> EntityActionsColumn { get; set; } = new();
       
        
        public PermissionItemTabs()
        {
            NewPermissionItem = new PermissionItemCreateDto();
            EditingPermissionItem = new PermissionItemUpdateDto();
            Filter = new GetPermissionItemsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            PermissionItemList = new List<PermissionItemDto>();
            PermissionRoleList = new List<PermissionRoleDto>();
        }

        protected override async Task OnInitializedAsync()
        {
          
            await SetPermissionsAsync();
            await GetPermissionRoleAsync();
        }

        
        private async Task GetPermissionRoleAsync()
        {
            PermissionRoleList = await PermissionExtensionsAppService.GetPermissionRoleListAsync();
        }
       

        private async Task SetPermissionsAsync()
        {
            CanCreatePermissionItem = await AuthorizationService
                .IsGrantedAsync(DataPermissionPermissions.PermissionItems.Create);
            CanEditPermissionItem = await AuthorizationService
                            .IsGrantedAsync(DataPermissionPermissions.PermissionItems.Edit);
            CanDeletePermissionItem = await AuthorizationService
                            .IsGrantedAsync(DataPermissionPermissions.PermissionItems.Delete);
        }

        private async Task GetPermissionItemsAsync()
        {
            if (ObjectName.IsNullOrWhiteSpace())
            {
                return;
            }
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;
            Filter.ObjectName = ObjectName;

            var result = await PermissionItemsAppService.GetListAsync(Filter);
            PermissionItemList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetPermissionItemsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await PermissionItemsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("DataPermission") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/data-permission/permission-items/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<PermissionItemDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetPermissionItemsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreatePermissionItemModalAsync()
        {
            NewPermissionItem = new PermissionItemCreateDto{
                ObjectName = ObjectName
                
            };
            await NewPermissionItemValidations.ClearAll();
            await CreatePermissionItemModal.Show();
        }

        private async Task CloseCreatePermissionItemModalAsync()
        {
            NewPermissionItem = new PermissionItemCreateDto{
                
                
            };
            await CreatePermissionItemModal.Hide();
        }

        private async Task OpenEditPermissionItemModalAsync(PermissionItemDto input)
        {
            var permissionItem = await PermissionItemsAppService.GetAsync(input.Id);
            
            EditingPermissionItemId = permissionItem.Id;
            EditingPermissionItem = ObjectMapper.Map<PermissionItemDto, PermissionItemUpdateDto>(permissionItem);
            await EditingPermissionItemValidations.ClearAll();
            await EditPermissionItemModal.Show();
        }

        private async Task DeletePermissionItemAsync(PermissionItemDto input)
        {
            await PermissionItemsAppService.DeleteAsync(input.Id);
            await GetPermissionItemsAsync();
        }

        private async Task CreatePermissionItemAsync()
        {
            try
            {
                if (await NewPermissionItemValidations.ValidateAll() == false)
                {
                    return;
                }

                await PermissionItemsAppService.CreateAsync(NewPermissionItem);
                await GetPermissionItemsAsync();
                await CloseCreatePermissionItemModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditPermissionItemModalAsync()
        {
            await EditPermissionItemModal.Hide();
        }

        private async Task UpdatePermissionItemAsync()
        {
            try
            {
                if (await EditingPermissionItemValidations.ValidateAll() == false)
                {
                    return;
                }

                await PermissionItemsAppService.UpdateAsync(EditingPermissionItemId, EditingPermissionItem);
                await GetPermissionItemsAsync();
                await EditPermissionItemModal.Hide();                
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CopyPermissionItemAsync(PermissionItemDto input)
        {
            try
            {
                await PermissionItemsAppService.CopyAsync(input.Id);
                await GetPermissionItemsAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

    }
}
