using JS.Abp.DataPermission.Permissions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using JS.Abp.DataPermission.Localization;
using Volo.Abp.UI.Navigation;

namespace JS.Abp.DataPermission.Blazor.Menus;

public class DataPermissionMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
            var moduleMenu = AddModuleMenuItem(context);
            AddMenuItemObjectPermissions(context, moduleMenu);
            //行级数据权限
            // AddMenuItemPermissionExtensions(context, moduleMenu);  
            //字段级数据权限
            // AddMenuItemPermissionItems(context, moduleMenu);
            AddMenuItemDemos(context, moduleMenu);

        }


    }

    private static async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        //Add main menu items.
        var l = context.GetLocalizer<DataPermissionResource>();

        //context.Menu.AddItem(new ApplicationMenuItem(DataPermissionMenus.Prefix, displayName: "Sample Page", "/DataPermission", icon: "fa fa-globe"));

        await Task.CompletedTask;
    }
    private static ApplicationMenuItem AddModuleMenuItem(MenuConfigurationContext context)
    {
        var moduleMenu = new ApplicationMenuItem(
            DataPermissionMenus.Prefix,
            context.GetLocalizer<DataPermissionResource>()["Menu:DataPermission"],
            icon: "fa fa-folder"
        );

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