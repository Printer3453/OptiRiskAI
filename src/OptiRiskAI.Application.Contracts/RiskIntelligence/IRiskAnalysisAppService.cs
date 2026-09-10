using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OptiRiskAI.RiskIntelligence
{
    // IApplicationService'den miras alarak ABP'ye bunun bir API'ye dönüşmesi gerektiğini söylüyoruz
    public interface IRiskAnalysisAppService : IApplicationService
    {
        // UI'dan çevresel verileri alıp, deterministik kurallarla risk çarpanını hesaplayacak ana metodumuz
        Task<RiskTelemetryDto> SubmitTelemetryAndAnalyzeAsync(CreateRiskTelemetryDto input);

        Task<List<RiskTelemetryDto>> GetLatestTelemetriesAsync();
    }
}
