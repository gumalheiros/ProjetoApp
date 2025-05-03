using ProjetoApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ProjetoApp.Permissions;

public class ProjetoAppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {

        var projectGroup = context.AddGroup(ProjetoAppPermissions.GroupName);

        var projectsPermission = projectGroup.AddPermission(ProjetoAppPermissions.Projects.Default);
        projectsPermission.AddChild(ProjetoAppPermissions.Projects.Create);
        projectsPermission.AddChild(ProjetoAppPermissions.Projects.Edit);
        projectsPermission.AddChild(ProjetoAppPermissions.Projects.Delete);
        projectsPermission.AddChild(ProjetoAppPermissions.Projects.ViewReports, L("Permission:ViewReports"));


        var customerPermission = projectGroup.AddPermission(ProjetoAppPermissions.Customer.Default, L("Permission:Customer"));
        customerPermission.AddChild(ProjetoAppPermissions.Customer.Create, L("Permission:Create"));
        customerPermission.AddChild(ProjetoAppPermissions.Customer.Update, L("Permission:Update"));
        customerPermission.AddChild(ProjetoAppPermissions.Customer.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ProjetoAppResource>(name);
    }
}
