using OptiRiskAI.EntityFrameworkCore;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace OptiRiskAI.RiskIntelligence
{
    
    public class RiskAnalysisAppService_Tests : OptiRiskAIEntityFrameworkCoreTestBase
    {
        private readonly IRiskAnalysisAppService _riskAnalysisAppService;

        public RiskAnalysisAppService_Tests()
        {
            
            _riskAnalysisAppService = GetRequiredService<IRiskAnalysisAppService>();
        }

        [Fact]
        public async Task Should_Calculate_High_Risk_For_Extreme_Wind_Speed()
        {
            var input = new CreateRiskTelemetryDto
            {
                Latitude = 41.02,
                Longitude = 28.98,
                DistanceToPowerLineMeters = 50,
                WindSpeedKmh = 65,
                SlopePercentage = 10,
                VegetationType = "Pine Forest"
            };

            var result = await _riskAnalysisAppService.SubmitTelemetryAndAnalyzeAsync(input);

            result.ShouldNotBeNull();
            result.IsProcessed.ShouldBeTrue();
            result.CalculatedRiskMultiplier.ShouldBe(2.5m);
        }
    }
}