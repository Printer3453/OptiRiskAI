using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace OptiRiskAI.RiskIntelligence
{
    public class RiskRuleDto: EntityDto<Guid>
    {
        public string RuleName { get; set; } = string.Empty;
        public string ConditionsJson { get; set; } = string.Empty;
        public decimal Multiplier { get; set; }
        public bool IsActive { get; set; }
       

    }
}
