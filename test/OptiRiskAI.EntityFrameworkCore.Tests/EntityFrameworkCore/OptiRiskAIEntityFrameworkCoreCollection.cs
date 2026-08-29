using Xunit;

namespace OptiRiskAI.EntityFrameworkCore;

[CollectionDefinition(OptiRiskAITestConsts.CollectionDefinitionName)]
public class OptiRiskAIEntityFrameworkCoreCollection : ICollectionFixture<OptiRiskAIEntityFrameworkCoreFixture>
{

}
