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
            // Koordinat Noktası (Boylam, Enlem sırasıyla girilir)
            var point =new Point(input.Latitude, input.Longitude);

            decimal calculatedRiskScore = 20.0m; // varsayılan risk skoru
            bool isInsideRiskArea = false;

            //  Poligon Geometrisi (Antalya/Manavgat Orman Hattı Simülasyonu
            var geometryFactory = new GeometryFactory();
            var coordinates = new[]
            {
                new Coordinate(31.0, 36.5),
                new Coordinate(32.0, 36.5),
                new Coordinate(32.0, 37.5),
                new Coordinate(31.0, 37.5),
                new Coordinate(31.0, 36.5) // Poligonun başlangıç ve bitiş noktası aynı olmalı
            };
            var riskPolygon = geometryFactory.CreatePolygon(coordinates);

            //  Nokta riskli poligonun içinde mi?
            if (riskPolygon.Intersects(point))
            {
                calculatedRiskScore = 85.0m;
                isInsideRiskArea = true;
            }

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

            telemetry.ApplyDeterminedRisk(Guid.Empty, calculatedRiskScore); // Risk skoru ve karar uygulanıyor
            await _telemetryRepository.InsertAsync(telemetry);// ENTITY KAYDI

            return new RiskTelemetryDto// DTO DÖNÜŞÜ
            {
                Id = telemetry.Id,
                Latitude = telemetry.Latitude,
                Longitude = telemetry.Longitude,
                CalculatedRiskMultiplier = calculatedRiskScore,
                IsProcessed = true
            };
        }

        
    }
}