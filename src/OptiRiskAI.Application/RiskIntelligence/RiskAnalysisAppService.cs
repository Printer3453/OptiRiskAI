using System;
using System.IO;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
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
            var geometryFactory = new NetTopologySuite.Geometries.GeometryFactory();
            var point = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(input.Longitude, input.Latitude));

            decimal calculatedRiskScore = 20.0m; // Ateşleme Olasılığı (Ignition)
            string spreadRisk = "Düşük (Güvenli Bölge)"; // Yayılım Modeli (Spread)

            var coordinates = new[]
            {
        new NetTopologySuite.Geometries.Coordinate(31.0, 36.5),
        new NetTopologySuite.Geometries.Coordinate(32.0, 36.5),
        new NetTopologySuite.Geometries.Coordinate(32.0, 37.5),
        new NetTopologySuite.Geometries.Coordinate(31.0, 37.5),
        new NetTopologySuite.Geometries.Coordinate(31.0, 36.5)
    };
            var riskPolygon = geometryFactory.CreatePolygon(coordinates);

            // Deterministik Matematik: Nokta riskli poligonun İÇİNDE mi?
            if (riskPolygon.Contains(point))
            {
                calculatedRiskScore = 85.0m;
                spreadRisk = "Kritik (Yüksek Eğim ve NDVI Endeksi)";
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