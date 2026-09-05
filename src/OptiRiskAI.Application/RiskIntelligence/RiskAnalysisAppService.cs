using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace OptiRiskAI.RiskIntelligence
{
    // ApplicationService'den miras alıp, Contracts katmanında yazdığımız arayüzü (Interface) uyguluyoruz
    public class RiskAnalysisAppService : ApplicationService, IRiskAnalysisAppService
    {
        private readonly IRepository<RiskTelemetry, Guid> _telemetryRepository;
        private readonly IRepository<RiskRule, Guid> _ruleRepository;

        // Dependency Injection (DI) - Veritabanı tablolarımızı aracı (Repository) desenle içeri alıyoruz
        public RiskAnalysisAppService(
            IRepository<RiskTelemetry, Guid> telemetryRepository,
            IRepository<RiskRule, Guid> ruleRepository)
        {
            _telemetryRepository = telemetryRepository;
            _ruleRepository = ruleRepository;
        }

        // Bu metod, kullanıcıdan gelen telemetri verilerini alır, bir RiskTelemetry entity'si oluşturur ve veritabanına kaydeder.
        public async Task<RiskTelemetryDto> SubmitTelemetryAndAnalyzeAsync(CreateRiskTelemetryDto input)
        {
            // 1. Kapsüllemeye (Encapsulation) uygun olarak Entity'mizi oluşturuyoruz
            // GuidGenerator.Create() ABP'nin sıralı (sequential) ve performanslı ID üreticisidir
            var telemetry = new RiskTelemetry(
                GuidGenerator.Create(),
                input.Latitude,
                input.Longitude,
                input.DistanceToPowerLineMeters,
                input.WindSpeedKmh,
                input.SlopePercentage,
                input.VegetationType
            );

            // 2. İlerleyen aşamada AI'ın ürettiği "RiskRule" matrisi burada devreye girip
            // telemetry.ApplyDeterminedRisk() metodunu tetikleyecek.
            // Şimdilik ham veriyi güvenli bir şekilde sisteme kaydediyoruz.
            await _telemetryRepository.InsertAsync(telemetry);

            // 3. Güvenlik için Veritabanı modelini (Entity), Taşıyıcı modele (DTO) çevirip dışarı dönüyoruz
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
