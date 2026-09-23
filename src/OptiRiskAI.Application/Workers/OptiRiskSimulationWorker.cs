using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;
using OptiRiskAI.RiskIntelligence;

namespace OptiRiskAI.Workers;

public class OptiRiskSimulationWorker : AsyncPeriodicBackgroundWorkerBase
{
    public OptiRiskSimulationWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory)
        : base(timer, serviceScopeFactory)
    {
        
        Timer.Period = 15000;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        Logger.LogInformation("=== Otonom Grid Taraması Başlatılıyor ===");

        // Simülasyon için Antalya/Manavgat bölgesinden örnek 3 grid koordinatı
        var simulationGrids = new[]
        {
            new { Lat = 36.85, Lon = 31.42, Name = "Orman İçi Merkez" },
            new { Lat = 36.81, Lon = 31.39, Name = "Güvenli Bölge (Şehir)" },
            new { Lat = 36.90, Lon = 31.45, Name = "Kritik Eğimli Tepe" }
        };

        // Arka plan görevlerinde Dependency Injection kapsamını manuel oluşturmalıyız
        var riskAppService = workerContext.ServiceProvider.GetRequiredService<IRiskAnalysisAppService>();
        var agentAppService = workerContext.ServiceProvider.GetRequiredService<IRiskActionAgentAppService>();

        foreach (var grid in simulationGrids)
        {
            Logger.LogInformation($"[TARANAN BÖLGE]: {grid.Name} ({grid.Lat}, {grid.Lon})");

            var telemetryInput = new CreateRiskTelemetryDto
            {
                Latitude = grid.Lat,
                Longitude = grid.Lon,
                DistanceToPowerLineMeters = 150,
                WindSpeedKmh = 20,
                SlopePercentage = 15,
                VegetationType = "Kızılçam"
            };

            try
            {
                
                var analysisResult = await riskAppService.SubmitTelemetryAndAnalyzeAsync(telemetryInput);
                Logger.LogInformation($"=> GIS/İklim Sonucu: Skor {analysisResult.CalculatedRiskMultiplier} | Durum: {analysisResult.SpreadRisk}");

                
                if (analysisResult.CalculatedRiskMultiplier > 0)
                {
                    var agentDecision = await agentAppService.EvaluateRiskScoreAndDecideActionAsync(
                        analysisResult.CalculatedRiskMultiplier ?? 0,
                        grid.Lat,
                        grid.Lon);

                    Logger.LogWarning($"=> [YAPAY ZEKA KARARI]: {agentDecision.ActionType} -> {agentDecision.AiReasoning}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Grid ({grid.Lat}, {grid.Lon}) taranırken bir hata oluştu. Simülasyon diğer grid ile devam ediyor.");
            }

            
            await Task.Delay(1000);
        }

        Logger.LogInformation("=== Grid Taraması Tamamlandı, Bekleniyor... ===");
    }
}