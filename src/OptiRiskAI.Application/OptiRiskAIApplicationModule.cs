using Microsoft.Extensions.DependencyInjection;
using OptiRiskAI.Workers;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace OptiRiskAI;

[DependsOn(
    typeof(OptiRiskAIDomainModule),
    typeof(OptiRiskAIApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class OptiRiskAIApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClient("OpenWeatherClient");
        context.Services.AddHttpClient("OpenMeteoClient");
    }
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
        context.AddBackgroundWorkerAsync<OptiRiskSimulationWorker>();
    }
}
