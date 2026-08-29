using Volo.Abp.Modularity;

namespace OptiRiskAI;

/* Inherit from this class for your domain layer tests. */
public abstract class OptiRiskAIDomainTestBase<TStartupModule> : OptiRiskAITestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
