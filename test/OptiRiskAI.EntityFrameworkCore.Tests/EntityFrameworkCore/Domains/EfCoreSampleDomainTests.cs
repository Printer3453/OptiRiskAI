using OptiRiskAI.Samples;
using Xunit;

namespace OptiRiskAI.EntityFrameworkCore.Domains;

[Collection(OptiRiskAITestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<OptiRiskAIEntityFrameworkCoreTestModule>
{

}
