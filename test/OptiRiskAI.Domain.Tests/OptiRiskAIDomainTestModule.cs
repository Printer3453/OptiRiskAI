using Volo.Abp.Modularity;

namespace OptiRiskAI;

[DependsOn(
    typeof(OptiRiskAIDomainModule),
    typeof(OptiRiskAITestBaseModule)
)]
public class OptiRiskAIDomainTestModule : AbpModule
{

}
