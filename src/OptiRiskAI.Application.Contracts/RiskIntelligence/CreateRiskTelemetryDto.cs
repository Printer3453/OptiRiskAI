using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OptiRiskAI.RiskIntelligence
{
    public class CreateRiskTelemetryDto
    {
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double DistanceToPowerLineMeters { get; set; }

        [Required]
        public double WindSpeedKmh { get; set; }

        [Required]
        public double SlopePercentage { get; set; }

        [Required]
        [MaxLength(64)]
        public string VegetationType { get; set; }
    }
}
