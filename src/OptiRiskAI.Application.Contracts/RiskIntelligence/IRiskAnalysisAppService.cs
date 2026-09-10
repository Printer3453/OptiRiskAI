using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OptiRiskAI.RiskIntelligence
{
    
    public interface IRiskAnalysisAppService : IApplicationService
    {
        
       // Task<RiskTelemetryDto> SubmitTelemetryAndAnalyzeAsync(CreateRiskTelemetryDto input);

        //Task<List<RiskTelemetryDto>> GetLatestTelemetriesAsync();
    }
}
