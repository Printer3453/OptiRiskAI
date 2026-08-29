using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace OptiRiskAI.Pages;

[Collection(OptiRiskAITestConsts.CollectionDefinitionName)]
public class Index_Tests : OptiRiskAIWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
