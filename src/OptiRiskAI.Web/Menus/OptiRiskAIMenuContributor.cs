using System.Threading.Tasks;
using OptiRiskAI.Localization;
using OptiRiskAI.Permissions;
using OptiRiskAI.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
namespace OptiRiskAI.Web.Menus;
public class OptiRiskAIMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }
    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<OptiRiskAIResource>();
        //Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                OptiRiskAIMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fa fa-home",
                order: 1
            )
        );
        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;
        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);
        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 8);

        context.Menu.AddItem(
            new ApplicationMenuItem(
                "BooksStore",
                l["Menu:OptiRiskAI"],
                icon: "fa fa-book"
            ).AddItem(
                new ApplicationMenuItem(
                    "BooksStore.Books",
                    l["Menu:Books"],
                    url: "/Books"
                ).RequirePermissions(OptiRiskAIPermissions.Books.Default)
            ).AddItem(
                new ApplicationMenuItem(
                    "BooksStore.Authors",
                    l["Menu:Authors"],
                    url: "/Authors"
                ).RequirePermissions(OptiRiskAIPermissions.Authors.Default)
            )
        );
        context.Menu.AddItem(
    new ApplicationMenuItem(
        "OptiRiskAI.AiRuleManager",
        "🤖 Otonom Kural Yöneticisi",
        url: "/AiRuleManager"
    )
);
        context.Menu.AddItem(
    new ApplicationMenuItem(
        "OptiRiskAI.AiRuleManager",
        "Canlı Risk Monitörü",
        url: "/RiskMonitor"
    )
);

        return Task.CompletedTask;
    }
}
