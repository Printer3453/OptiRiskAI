using System;
using System.Collections.Generic;
using System.Text;

namespace OptiRiskAI.OpenMeteo
{
    public class WeatherTelemetryDto
    {
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float WindSpeed { get; set; }
        public float Rain { get; set; }
    }
}
