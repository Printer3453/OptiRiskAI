using OptiRiskAI.Samples;
using Xunit;

namespace OptiRiskAI.EntityFrameworkCore.Applications;

[Collection(OptiRiskAITestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<OptiRiskAIEntityFrameworkCoreTestModule>
{

}
