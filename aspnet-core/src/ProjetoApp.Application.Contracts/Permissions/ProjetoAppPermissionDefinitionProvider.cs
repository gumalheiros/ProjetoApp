using ProjetoApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ProjetoApp.Permissions;

public class ProjetoAppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ProjetoAppPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ProjetoAppPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ProjetoAppResource>(name);
    }
}
