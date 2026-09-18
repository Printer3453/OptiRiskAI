using System.Threading.Tasks;
using OptiRiskAI.OpenWeather;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;

namespace OptiRiskAI.RiskAnalysis;

public class RiskAnalysisAppService : ApplicationService, IRiskAnalysisAppService
{
    private readonly IOpenWeatherService _openWeatherService;

    // Dependency Injection ile yazdığımız dış API servisini içeri alıyoruz
    public RiskAnalysisAppService(IOpenWeatherService openWeatherService)
    {
        _openWeatherService = openWeatherService;
    }


    // burada latitude ve longitude parametreleri ile risk analizi yapılacak
    public async Task<RiskAnalysisResultDto> AnalyzeLocationAsync(double latitude, double longitude)
    {
        // 1. GIS Kontrolü (Önce Coğrafya)
        bool isInsideRiskZone = CheckIfInRiskZone(latitude, longitude);

        if (!isInsideRiskZone)
        {
            Logger.LogInformation($"[GIS FİLTRESİ] Koordinatlar ({latitude}, {longitude}) güvenli bölgede. API isteği atlanıyor.");

            return new RiskAnalysisResultDto
            {
                IsRiskDetected = false,
                FwiScore = 0,
                Message = "Seçilen lokasyon orman risk poligonu dışındadır. Güvenli."
            };
        }

        // 2. Eğer riskli bölgedeyse OpenWeather API'den gerçek zamanlı FWI Skorunu çek
        Logger.LogInformation($"[GIS FİLTRESİ] Koordinatlar ({latitude}, {longitude}) riskli bölgede! OpenWeather FWI API tetikleniyor...");

        double fwiScore = await _openWeatherService.GetFwiScoreAsync(latitude, longitude);

        return new RiskAnalysisResultDto
        {
            IsRiskDetected = true,
            FwiScore = fwiScore,
            Message = fwiScore >= 70 ? "Kritik Yangın Riski Tespit Edildi!" : "İklim Şartları Risk Sınırları İçerisinde."
        };
    }

    // NetTopologySuite Point-in-Polygon algoritmasının entegre edileceği metod
    private bool CheckIfInRiskZone(double latitude, double longitude)
    {
        // NOT: İlerleyen aşamada buraya GeoJSON okuma ve .Contains() mantığını ekleyeceğiz.
        // Şimdilik sistemin kesintisiz çalışması için Antalya/Manavgat koordinatlarına 
        // basit bir 'Bounding Box' (sınır kutusu) simülasyonu koyuyoruz.
        return latitude is >= 36.5 and <= 37.5 && longitude is >= 30.5 and <= 32.0;
    }
}