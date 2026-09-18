using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.IO.Converters;
using OptiRiskAI.OpenWeather; 
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace OptiRiskAI.RiskIntelligence
{
    public class RiskAnalysisAppService : ApplicationService, IRiskAnalysisAppService
    {
        private readonly IRepository<RiskTelemetry, Guid> _telemetryRepository;
        private readonly IOpenWeatherService _openWeatherService; // Dış API servisi

        public RiskAnalysisAppService(
            IRepository<RiskTelemetry, Guid> telemetryRepository,
            IOpenWeatherService openWeatherService)
        {
            _telemetryRepository = telemetryRepository;
            _openWeatherService = openWeatherService;
        }

        public async Task<RiskTelemetryDto> SubmitTelemetryAndAnalyzeAsync(CreateRiskTelemetryDto input)
        {
            var geometryFactory = new NetTopologySuite.Geometries.GeometryFactory();
            var point = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(input.Longitude, input.Latitude));

            // Varsayılan değerler (Güvenli Bölge)
            decimal calculatedRiskScore = 0.0m;
            string spreadRisk = "Düşük (Güvenli Bölge)";

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
                        // 1. AŞAMA: Coğrafi (GIS) Filtreleme
                        if (feature.Geometry.Contains(point))
                        {
                            isInsideRiskZone = true;
                            spreadRisk = feature.Attributes["riskType"]?.ToString() ?? "Kritik (Yüksek Eğim)";
                            break;
                        }
                    }
                }

                // 2. AŞAMA: Dış Veri (Meteoroloji) Entegrasyonu
                if (isInsideRiskZone)
                {
                    Logger.LogInformation($"[GIS ONAYI] Koordinat orman içinde. OpenWeather FWI API tetikleniyor...");

                    // Sahte 85 skoru yerine gerçek zamanlı API verisini çekiyoruz
                    double fwiScore = await _openWeatherService.GetFwiScoreAsync(input.Latitude, input.Longitude);
                    calculatedRiskScore = (decimal)fwiScore;
                }
                else
                {
                    Logger.LogInformation($"[GIS REDDİ] Koordinat orman dışında. API isteği atlandı.");
                }
            }
            else
            {
                spreadRisk = "Sistem Uyarısı: GeoJSON veritabanına ulaşılamadı!";
            }

            // 3. AŞAMA: Veritabanı Kaydı
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
                IsProcessed = true
            };
        }
    }
}