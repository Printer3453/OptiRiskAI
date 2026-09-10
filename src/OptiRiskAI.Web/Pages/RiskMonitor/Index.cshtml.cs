using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OptiRiskAI.RiskIntelligence;

namespace OptiRiskAI.Web.Pages.RiskMonitor
{
    public class IndexModel : PageModel
    {
        private readonly IRiskAnalysisAppService _riskAnalysisAppService;

        public List<RiskTelemetryDto> Telemetries { get; set; } = new();

        public IndexModel(IRiskAnalysisAppService riskAnalysisAppService)
        {
            _riskAnalysisAppService = riskAnalysisAppService;
        }

        public async Task OnGetAsync()
        {
            
            Telemetries = await _riskAnalysisAppService.GetLatestTelemetriesAsync();
        }
    }
}