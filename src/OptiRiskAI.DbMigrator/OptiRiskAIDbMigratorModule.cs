using OptiRiskAI.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace OptiRiskAI.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(OptiRiskAIEntityFrameworkCoreModule),
    typeof(OptiRiskAIApplicationContractsModule)
)]
public class OptiRiskAIDbMigratorModule : AbpModule
{
}
