using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace OptiRiskAI.RiskIntelligence
{
    public class RiskTelemetryDto : AuditedEntityDto<Guid>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double DistanceToPowerLineMeters { get; set; }
        public double WindSpeedKmh { get; set; }
        public double SlopePercentage { get; set; }
        public string VegetationType { get; set; }

        public Guid? AppliedRiskRuleId { get; set; }
        public decimal? CalculatedRiskMultiplier { get; set; }
        public bool IsProcessed { get; set; }
    }
}
