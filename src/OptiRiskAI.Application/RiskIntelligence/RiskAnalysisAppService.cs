using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using NCalc;

namespace OptiRiskAI.RiskIntelligence
{
    public class RiskAnalysisAppService : ApplicationService, IRiskAnalysisAppService
    {
        private readonly IRepository<RiskTelemetry, Guid> _telemetryRepository;
        private readonly IRepository<RiskRule, Guid> _ruleRepository;

        public RiskAnalysisAppService(IRepository<RiskTelemetry, Guid> telemetryRepository, IRepository<RiskRule, Guid> ruleRepository)
        {
            _telemetryRepository = telemetryRepository;
            _ruleRepository = ruleRepository;
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

            decimal finalMultiplier = 1.0m;
            Guid? appliedRuleId = null;
            var activeRules = await _ruleRepository.GetListAsync(r => r.IsActive);

            //  Kural Motoru: AI'ın ürettiği kuralları dinamik olarak test ediyoruz
            foreach (var rule in activeRules)
            {
                try
                {
                    // AI'ın ürettiği koşulu NCalc'in sorunsuz okuyabilmesi için ufak bir syntax temizliği
                    var safeExpression = rule.ConditionExpression
                        .Replace("AND", "&&")
                        .Replace("OR", "||");

                    var expression = new Expression(safeExpression);
                    // Sahadan gelen telemetri verilerini dinamik kurala parametre olarak enjekte ediyoruz
                    expression.Parameters["WindSpeedKmh"] = input.WindSpeedKmh;
                    expression.Parameters["DistanceToPowerLineMeters"] = input.DistanceToPowerLineMeters;
                    expression.Parameters["SlopePercentage"] = input.SlopePercentage;
                    expression.Parameters["VegetationType"] = input.VegetationType;

                    // AI'ın yazdığı kuralı C# kodunda anlık olarak (Runtime) çalıştırıyoruz
                    var isMatch = Convert.ToBoolean(expression.Evaluate());

                    // Eğer sahadaki koşullar kuralı karşılıyorsa ve risk çarpanı eskisinden yüksekse, bunu geçerli kural yap
                    if (isMatch&&rule.RiskMultiplier>finalMultiplier)
                    {
                        finalMultiplier = rule.RiskMultiplier;
                        appliedRuleId = rule.Id;
                    }

                }
                catch (Exception ex)
                {
                    // Log kayıtlarını daha sonra yapacağız UNUTMA!!! Şimdilik hata vermesin 
                    continue;
                }
            }

            telemetry.ApplyDeterminedRisk(appliedRuleId ?? Guid.Empty, finalMultiplier);

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