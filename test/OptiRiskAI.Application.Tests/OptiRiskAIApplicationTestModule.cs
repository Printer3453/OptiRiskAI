using Volo.Abp.Modularity;

namespace OptiRiskAI;

[DependsOn(
    typeof(OptiRiskAIApplicationModule),
    typeof(OptiRiskAIDomainTestModule)
)]
public class OptiRiskAIApplicationTestModule : AbpModule
{

}
