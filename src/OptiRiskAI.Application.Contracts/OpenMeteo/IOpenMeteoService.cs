using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OptiRiskAI.OpenMeteo
{
    public interface IOpenMeteoService : IApplicationService
    {
        Task<WeatherTelemetryDto> GetCurrentWeatherAsync(double latitude, double longitude);
    
    }
}
