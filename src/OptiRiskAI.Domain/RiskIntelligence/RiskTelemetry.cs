using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace OptiRiskAI.RiskIntelligence
{
    public class RiskTelemetry : FullAuditedAggregateRoot<Guid>
    {
        // Haritada (Leaflet/Mapbox) göstereceğimiz koordinatlar
        public double Latitude { get; private set; }// enlem (latitude) ve boylam (longitude) değerleri, sahadaki telemetri verisinin coğrafi konumunu temsil eder. Örn: 41.0082, 28.9784
        public double Longitude { get; private set; }

        // Çevresel Sensör / Gözlem Verileri
        public double DistanceToPowerLineMeters { get; private set; }// elektrik hattına olan mesafeyi metre cinsinden gösterir. Örn: 15.5
        public double WindSpeedKmh { get; private set; }// rüzgar hızını kilometre/saat cinsinden gösterir. Örn: 45.0
        public double SlopePercentage { get; private set; }// arazi eğimini yüzde cinsinden gösterir. Örn: 12.5, yüzde ne kadar yüksek ise , risk o kadar artar.
        public string VegetationType { get; private set; } // Örn: "Kızılçam", "Maki", "Seyrek Ot"

        // Verileri yapay zekanın Kural Matrisi ile eşleştirdiğinde burası dolacak
        public Guid? AppliedRiskRuleId { get; private set; }// AI tarafından eşleşen kuralın ID'si, eğer sahadaki telemetri verisi herhangi bir risk kuralına uyuyorsa, bu alan o kuralın benzersiz kimliğini (ID) tutar. Örn: "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        public decimal? CalculatedRiskMultiplier { get; private set; }// AI tarafından hesaplanan risk çarpanı, eğer sahadaki telemetri verisi bir risk kuralına uyuyorsa, bu alan o kuralın belirlediği risk çarpanını tutar. Örn: 2.5
        public bool IsProcessed { get; private set; }// AI tarafından işlenip işlenmediğini gösterir, eğer sahadaki telemetri verisi henüz herhangi bir risk kuralına karşı değerlendirilmemişse, bu alan false olur. Örn: false

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
