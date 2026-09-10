using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OptiRiskAI.RiskIntelligence
{
    public class RiskAnalysisAppService : ApplicationService, IRiskAnalysisAppService
    {
        private readonly IRepository<RiskTelemetry, Guid> _telemetryRepository;

        public RiskAnalysisAppService(IRepository<RiskTelemetry, Guid> telemetryRepository)
        {
            _telemetryRepository = telemetryRepository;
        }

        public async Task<RiskTelemetryDto> SubmitTelemetryAndAnalyzeAsync(CreateRiskTelemetryDto input)
        {
            var telemetry = new RiskTelemetry(
                GuidGenerator.Create(),
                input.Latitude,
                input.Longitude,
                input.DistanceToPowerLineMeters,
                input.WindSpeedKmh,
                input.SlopePercentage,
                input.VegetationType
            );

            // Basit bir risk çarpanı hesaplama mantığı ekliyoruz. Bu, MVP aşamasında kullanılacak ve daha sonra dinamik kurallar ile değiştirilebilir.
            decimal calculatedMultiplier = 1.0m;

            if (input.WindSpeedKmh > 50 || input.SlopePercentage > 30)
            {
                calculatedMultiplier = 2.5m;
            }
            if (input.DistanceToPowerLineMeters < 10)
            {
                calculatedMultiplier += 1.5m;
            }

            telemetry.ApplyDeterminedRisk(Guid.Empty, calculatedMultiplier);

            await _telemetryRepository.InsertAsync(telemetry);

            return new RiskTelemetryDto
            {
                Id = telemetry.Id,
                Latitude = telemetry.Latitude,
                Longitude = telemetry.Longitude,
                DistanceToPowerLineMeters = telemetry.DistanceToPowerLineMeters,
                WindSpeedKmh = telemetry.WindSpeedKmh,
                SlopePercentage = telemetry.SlopePercentage,
                VegetationType = telemetry.VegetationType,
                AppliedRiskRuleId = telemetry.AppliedRiskRuleId,
                CalculatedRiskMultiplier = telemetry.CalculatedRiskMultiplier,
                IsProcessed = telemetry.IsProcessed
            };
        }
    }
}