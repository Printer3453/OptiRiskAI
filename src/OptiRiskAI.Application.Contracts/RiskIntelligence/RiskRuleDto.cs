using System;
using Volo.Abp.Application.Dtos;

namespace OptiRiskAI.RiskIntelligence
{
    public class RiskRuleDto : EntityDto<Guid>
    {
        public string RuleName { get; set; } = string.Empty;
        public string ConditionExpression { get; set; } = string.Empty;
        public decimal RiskMultiplier { get; set; }
        public string AiRationale { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}