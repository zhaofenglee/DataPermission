using JS.Abp.DataPermission.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace JS.Abp.DataPermission.Permissions;

public class DataPermissionPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(DataPermissionPermissions.GroupName, L("Permission:DataPermission"));

        var permissionExtensionPermission = myGroup.AddPermission(DataPermissionPermissions.PermissionExtensions.Default, L("Permission:PermissionExtensions"));
        permissionExtensionPermission.AddChild(DataPermissionPermissions.PermissionExtensions.Create, L("Permission:Create"));
        permissionExtensionPermission.AddChild(DataPermissionPermissions.PermissionExtensions.Edit, L("Permission:Edit"));
        permissionExtensionPermission.AddChild(DataPermissionPermissions.PermissionExtensions.Delete, L("Permission:Delete"));

        var demoPermission = myGroup.AddPermission(DataPermissionPermissions.Demos.Default, L("Permission:Demos"));
        demoPermission.AddChild(DataPermissionPermissions.Demos.Create, L("Permission:Create"));
        demoPermission.AddChild(DataPermissionPermissions.Demos.Edit, L("Permission:Edit"));
        demoPermission.AddChild(DataPermissionPermissions.Demos.Delete, L("Permission:Delete"));

        var objectPermissionPermission = myGroup.AddPermission(DataPermissionPermissions.ObjectPermissions.Default, L("Permission:ObjectPermissions"));
        objectPermissionPermission.AddChild(DataPermissionPermissions.ObjectPermissions.Create, L("Permission:Create"));
        objectPermissionPermission.AddChild(DataPermissionPermissions.ObjectPermissions.Edit, L("Permission:Edit"));
        objectPermissionPermission.AddChild(DataPermissionPermissions.ObjectPermissions.Delete, L("Permission:Delete"));

        var permissionItemPermission = myGroup.AddPermission(DataPermissionPermissions.PermissionItems.Default, L("Permission:PermissionItems"));
        permissionItemPermission.AddChild(DataPermissionPermissions.PermissionItems.Create, L("Permission:Create"));
        permissionItemPermission.AddChild(DataPermissionPermissions.PermissionItems.Edit, L("Permission:Edit"));
        permissionItemPermission.AddChild(DataPermissionPermissions.PermissionItems.Delete, L("Permission:Delete"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DataPermissionResource>(name);
    }
}