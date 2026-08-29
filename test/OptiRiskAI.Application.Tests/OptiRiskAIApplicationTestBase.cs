using Volo.Abp.Modularity;

namespace OptiRiskAI;

public abstract class OptiRiskAIApplicationTestBase<TStartupModule> : OptiRiskAITestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
