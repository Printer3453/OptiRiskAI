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
            
            // 0-100 arası bir İklim/Yangın Risk Skoru hesaplıyoruz.

            decimal baseRiskScore = 10.0m;

            // Rüzgar Çarpanı: 50 km/h üzeri her kilometre için riski artır
            decimal windFactor = input.WindSpeedKmh > 50 ? (decimal)(input.WindSpeedKmh - 50) * 0.8m : 0;

            // Eğim Çarpanı: Eğimi yüksek arazide yangının hızı ve müdahale zorluğu artar
            decimal slopeFactor = (decimal)input.SlopePercentage * 0.5m;

            // Bitki Örtüsü / NDMI (Kuruma) Simülasyonu
            decimal vegetationFactor = input.VegetationType.Contains("Çam", StringComparison.OrdinalIgnoreCase) ||
                                       input.VegetationType.Contains("Pine", StringComparison.OrdinalIgnoreCase)
                                       ? 30.0m : 10.0m;

            // Toplam Skor: Bütün risk faktörlerini topla ve 100'e sabitle (Clamp)
            decimal rawScore = baseRiskScore + windFactor + slopeFactor + vegetationFactor;
            decimal calculatedRiskScore = Math.Min(Math.Max(rawScore, 0), 100);

            //  ENTITY KAYDI
            var telemetry = new RiskTelemetry(
                GuidGenerator.Create(),
                input.Latitude,
                input.Longitude,
                input.DistanceToPowerLineMeters,
                input.WindSpeedKmh,
                input.SlopePercentage,
                input.VegetationType
            );

            // Domain Entity'mizdeki Multiplier alanını şimdilik "100 Üzerinden Skor" olarak kullanıyoruz.
            // (İleride Entity'de bu alanı 'RiskScore' olarak adlandırabiliriz)
            telemetry.ApplyDeterminedRisk(Guid.Empty, calculatedRiskScore);

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
                CalculatedRiskMultiplier = telemetry.CalculatedRiskMultiplier, // 0-100 arası skor dönüyor
                IsProcessed = telemetry.IsProcessed
            };
        }

        
    }
}