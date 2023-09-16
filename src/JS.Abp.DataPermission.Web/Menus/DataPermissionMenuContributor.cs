using JS.Abp.DataPermission.Permissions;
using JS.Abp.DataPermission.Localization;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Volo.Abp.Authorization.Permissions;

namespace JS.Abp.DataPermission.Web.Menus;

public class DataPermissionMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return;
        }

        var moduleMenu = AddModuleMenuItem(context); //Do not delete `moduleMenu` variable as it will be used by ABP Suite!

        //禁用行级和字段级维护菜单，统一使用对象数据权限
        AddMenuItemObjectPermissions(context, moduleMenu);

        //AddMenuItemPermissionExtensions(context, moduleMenu);

        //AddMenuItemPermissionItems(context, moduleMenu);

       
        AddMenuItemDemos(context, moduleMenu);
    }

    private static ApplicationMenuItem AddModuleMenuItem(MenuConfigurationContext context)
    {
        var moduleMenu = new ApplicationMenuItem(
            DataPermissionMenus.Prefix,
            displayName: context.GetLocalizer<DataPermissionResource>()["Menu:DataPermission"],
            icon: "fa fa-globe");

        //Add main menu items.
        context.Menu.Items.AddIfNotContains(moduleMenu);
        return moduleMenu;
    }
    private static void AddMenuItemPermissionExtensions(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.DataPermissionMenus.PermissionExtensions,
                context.GetLocalizer<DataPermissionResource>()["Menu:PermissionExtensions"],
                "/DataPermission/PermissionExtensions",
                icon: "fa fa-file-alt",
                requiredPermissionName: DataPermissionPermissions.PermissionExtensions.Default
            )
        );
    }

    private static void AddMenuItemDemos(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.DataPermissionMenus.Demos,
                context.GetLocalizer<DataPermissionResource>()["Menu:Demos"],
                "/DataPermission/Demos",
                icon: "fa fa-file-alt",
                requiredPermissionName: DataPermissionPermissions.Demos.Default
            )
        );
    }

    private static void AddMenuItemPermissionItems(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.DataPermissionMenus.PermissionItems,
                context.GetLocalizer<DataPermissionResource>()["Menu:PermissionItems"],
                "/DataPermission/PermissionItems",
                icon: "fa fa-file-alt",
                requiredPermissionName: DataPermissionPermissions.PermissionItems.Default
            )
        );
    }

    private static void AddMenuItemObjectPermissions(MenuConfigurationContext context, ApplicationMenuItem parentMenu)
    {
        parentMenu.AddItem(
            new ApplicationMenuItem(
                Menus.DataPermissionMenus.ObjectPermissions,
                context.GetLocalizer<DataPermissionResource>()["Menu:ObjectPermissions"],
                "/DataPermission/ObjectPermissions",
                icon: "fa fa-file-alt",
                requiredPermissionName: DataPermissionPermissions.ObjectPermissions.Default
            )
        );
    }
}