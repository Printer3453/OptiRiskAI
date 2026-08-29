using OptiRiskAI.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace OptiRiskAI.Permissions;

public class OptiRiskAIPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(OptiRiskAIPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(OptiRiskAIPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(OptiRiskAIPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(OptiRiskAIPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(OptiRiskAIPermissions.Books.Delete, L("Permission:Books.Delete"));

        var authorsPermission = myGroup.AddPermission(OptiRiskAIPermissions.Authors.Default, L("Permission:Authors"));
        authorsPermission.AddChild(OptiRiskAIPermissions.Authors.Create, L("Permission:Authors.Create"));
        authorsPermission.AddChild(OptiRiskAIPermissions.Authors.Edit, L("Permission:Authors.Edit"));
        authorsPermission.AddChild(OptiRiskAIPermissions.Authors.Delete, L("Permission:Authors.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(OptiRiskAIPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OptiRiskAIResource>(name);
    }
}
