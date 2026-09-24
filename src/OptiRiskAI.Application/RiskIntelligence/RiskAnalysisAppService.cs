using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.IO.Converters;
using OptiRiskAI.OpenMeteo;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.Extensions.Logging;
using OptiRiskAI_Application;



namespace OptiRiskAI.RiskIntelligence
{
    public class RiskAnalysisAppService : ApplicationService, IRiskAnalysisAppService
    {
        private readonly IRepository<RiskTelemetry, Guid> _telemetryRepository;
        private readonly IOpenMeteoService _openMeteoService; 

        public RiskAnalysisAppService(
            IRepository<RiskTelemetry, Guid> telemetryRepository,
            IOpenMeteoService openMeteoService) 
        {
            _telemetryRepository = telemetryRepository;
            _openMeteoService = openMeteoService;
        }

        public async Task<RiskTelemetryDto> SubmitTelemetryAndAnalyzeAsync(CreateRiskTelemetryDto input)
        {
            var geometryFactory = new NetTopologySuite.Geometries.GeometryFactory();
            var point = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(input.Longitude, input.Latitude));

            // Varsayılan değerler (Güvenli Bölge)
            decimal calculatedRiskScore = 0.0m;
            string spreadRisk = "Düşük (Güvenli Bölge)";
            float? currentTemp = null; 
            float? currentHum = null;

            var geoJsonPath = Path.Combine(AppContext.BaseDirectory, "GeoData", "AntalyaRisk.geojson");

            if (File.Exists(geoJsonPath))
            {
                string geoJsonText = await File.ReadAllTextAsync(geoJsonPath);

                var options = new JsonSerializerOptions();
                options.Converters.Add(new GeoJsonConverterFactory(geometryFactory));
                var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(geoJsonText, options);

                bool isInsideRiskZone = false;

                if (featureCollection != null)
                {
                    foreach (var feature in featureCollection)
                    {
                        // Coğrafi (GIS) Filtreleme
                        if (feature.Geometry.Contains(point))
                        {
                            isInsideRiskZone = true;
                            spreadRisk = feature.Attributes["riskType"]?.ToString() ?? "Kritik (Yüksek Eğim)";
                            break;
                        }
                    }
                }

                // ML.NET ve Open-Meteo Entegrasyonu 
                if (isInsideRiskZone)
                {
                    Logger.LogInformation($"[GIS ONAYI] Koordinat orman içinde. Open-Meteo verisi çekiliyor...");

                    // Open-Meteo'dan ücretsiz ve limitsiz anlık hava durumunu çek
                    var weatherData = await _openMeteoService.GetCurrentWeatherAsync(input.Latitude, input.Longitude);
                    currentTemp = weatherData.Temperature; 
                    currentHum = weatherData.Humidity;

                    Logger.LogInformation($"[METEO] Sıcaklık: {weatherData.Temperature}°C, Nem: %{weatherData.Humidity}, Rüzgar: {weatherData.WindSpeed}km/s");

                    // Gelen canlı veriyi ML.NET motorumuza besle
                    var modelInput = new OptiRiskModel.ModelInput()
                    {
                        Temperature = weatherData.Temperature,
                        RH = weatherData.Humidity,
                        Ws = weatherData.WindSpeed,
                        Rain = weatherData.Rain
                    };

                    // Öğrettiğimiz model ile Aktüeryal Riski Tahmin Et
                    var prediction = OptiRiskModel.Predict(modelInput);
                    calculatedRiskScore = (decimal)prediction.Score;
                    
                    Logger.LogInformation($"[ML.NET] Aktüeryal Risk Skoru (FWI) Üretildi: {calculatedRiskScore}");
                    
                    // Otonom Ajan Tetikleme Ön Hazırlık
                    if(calculatedRiskScore > 30.0m)
                    {
                         Logger.LogWarning($"[AJAN TETİKLENDİ] Yüksek risk ({Math.Round(calculatedRiskScore, 1)}). BLOCK_POLICY kararı alındı.");
                         spreadRisk += $" | ML.NET UYARISI: Kritik FWI Skoru ({Math.Round(calculatedRiskScore, 1)})";
                    }
                }
                else
                {
                    Logger.LogInformation($"[GIS REDDİ] Koordinat orman dışında. Meteoroloji ve ML modeli pas geçildi.");
                }
            }
            else
            {
                spreadRisk = "Sistem Uyarısı: GeoJSON veritabanına ulaşılamadı!";
            }

            // Veritabanı Kaydı (Mevcut kodun korundu)
            var telemetry = new RiskTelemetry(
                GuidGenerator.Create(),
                input.Latitude,
                input.Longitude,
                input.DistanceToPowerLineMeters,
                input.WindSpeedKmh,
                input.SlopePercentage,
                input.VegetationType
            );

            telemetry.ApplyDeterminedRisk(Guid.Empty, calculatedRiskScore);
            await _telemetryRepository.InsertAsync(telemetry);

            return new RiskTelemetryDto
            {
                Id = telemetry.Id,
                Latitude = telemetry.Latitude,
                Longitude = telemetry.Longitude,
                CalculatedRiskMultiplier = calculatedRiskScore,
                SpreadRisk = spreadRisk,
                IsProcessed = true,
                Temperature = currentTemp, 
                Humidity = currentHum
            };
        }
    }
}