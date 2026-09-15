using System;
using System.IO;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Text.Json;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using NetTopologySuite.IO.Converters;




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
            var geometryFactory = new NetTopologySuite.Geometries.GeometryFactory();
            var point = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(input.Longitude, input.Latitude));

            decimal calculatedRiskScore = 20.0m;
            string spreadRisk = "Düşük (Güvenli Bölge)";

            // 1. GERÇEK VERİ OKUMA: Hardcode bitti, Enterprise GIS entegrasyonu başladı!
            var geoJsonPath = Path.Combine(AppContext.BaseDirectory, "GeoData", "AntalyaRisk.geojson");

            if (File.Exists(geoJsonPath))
            {
                string geoJsonText = await File.ReadAllTextAsync(geoJsonPath);

                // 2. GeoJSON Parser Ayarları (.NET 8 Text.Json uyumlu)
                var options = new JsonSerializerOptions();
                options.Converters.Add(new GeoJsonConverterFactory(geometryFactory));

                // 3. Dosyayı C# Feature Collection objesine dönüştür
                var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(geoJsonText, options);

                if (featureCollection != null)
                {
                    // 4. Binlerce poligon olsa bile hepsini döner ve noktanın poligon içinde olup olmadığını bulur
                    foreach (var feature in featureCollection)
                    {
                        if (feature.Geometry.Contains(point))
                        {
                            calculatedRiskScore = 85.0m;
                            // GeoJSON içindeki gerçek 'properties' verisini (Örn: Kritik Yayılım) dinamik olarak alabiliriz
                            spreadRisk = feature.Attributes["riskType"]?.ToString() ?? "Kritik (Yüksek Eğim)";
                            break; // Riski bulduk, diğer poligonlara bakmaya gerek yok
                        }
                    }
                }
            }
            else
            {
                // Dosya bulunamazsa sistemi çökertme, logla ve güvenli skordan devam et
                spreadRisk = "Sistem Uyarısı: GeoJSON veritabanına ulaşılamadı!";
            }

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