using OptiRiskAI.Books;
using Xunit;

namespace OptiRiskAI.EntityFrameworkCore.Applications.Books;

[Collection(OptiRiskAITestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<OptiRiskAIEntityFrameworkCoreTestModule>
{

}