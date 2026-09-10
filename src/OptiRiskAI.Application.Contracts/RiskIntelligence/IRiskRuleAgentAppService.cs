using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OptiRiskAI.RiskIntelligence
{
    public interface IRiskRuleAgentAppService: IApplicationService
    {
        // Yöneticiden gelen doğal dil metnini(Prompt) alıp, 
        // yapay zeka ile işleyerek sisteme yeni bir Risk Kuralı (RiskRule) kazandıracak metod.
        Task<RiskRuleDto> GenerateRiskRuleFromPromptAsync(GenerateRuleFromPromptDto input);
    }
}
