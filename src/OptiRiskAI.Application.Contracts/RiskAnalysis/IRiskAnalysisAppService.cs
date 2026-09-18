using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OptiRiskAI.RiskAnalysis;

public interface IRiskAnalysisAppService : IApplicationService
{
    Task<RiskAnalysisResultDto> AnalyzeLocationAsync(double latitude, double longitude);
}