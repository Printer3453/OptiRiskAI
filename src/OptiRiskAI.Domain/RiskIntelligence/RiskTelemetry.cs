using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace OptiRiskAI.RiskIntelligence
{
    public class RiskTelemetry : FullAuditedAggregateRoot<Guid>
    {
        // Haritada (Leaflet/Mapbox) göstereceğimiz koordinatlar
        public double Latitude { get; private set; }
        public double Latitude { get; private set; }

        // Çevresel Sensör / Gözlem Verileri
        public double DistanceToPowerLineMeters { get; private set; }
        public double WindSpeedKmh { get; private set; }
        public double SlopePercentage { get; private set; }
        public string VegetationType { get; private set; } // Örn: "Kızılçam", "Maki", "Seyrek Ot"

        // Verileri yapay zekanın Kural Matrisi ile eşleştirdiğinde burası dolacak
        public Guid? AppliedRiskRuleId { get; private set; }
        public decimal? CalculatedRiskMultiplier { get; private set; }
        public bool IsProcessed { get; private set; }

        private RiskTelemetry()
        {
        }
        public RiskTelemetry(
            Guid id,
            double latitude,
            double longitude,
            double distanceToPowerLineMeters,
            double windSpeedKmh,
            double slopePercentage,
            string vegetationType)
            : base(id)
        {
            Latitude = latitude;
            Longitude = longitude;
            DistanceToPowerLineMeters = distanceToPowerLineMeters;
            WindSpeedKmh = windSpeedKmh;
            SlopePercentage = slopePercentage;
            VegetationType = vegetationType;
            IsProcessed = false;
        }

        
        public void ApplyDeterminedRisk(Guid ruleId, decimal multiplier)
        {
            AppliedRiskRuleId = ruleId;
            CalculatedRiskMultiplier = multiplier;
            IsProcessed = true;
        }

    }
}
