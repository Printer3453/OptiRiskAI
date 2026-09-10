using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OptiRiskAI.RiskIntelligence
{
    public interface IRiskActionAgentAppService : IApplicationService
    {
        
        Task<RiskActionDecisionDto> EvaluateRiskScoreAndDecideActionAsync(decimal calculatedScore, double latitude, double longitude);
    }
}
