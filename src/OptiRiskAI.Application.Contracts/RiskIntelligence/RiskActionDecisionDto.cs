using System;
using System.Collections.Generic;
using System.Text;

namespace OptiRiskAI.RiskIntelligence
{
    public class RiskActionDecisionDto
    {
        public string ActionType { get; set; } = string.Empty; // Örn: "BLOCK_POLICY", "ALERT_TEAM", "LOG_ONLY"
        public string TargetDepartment { get; set; } = string.Empty; // Örn: "Underwriting", "Maintenance"
        public string AiReasoning { get; set; } = string.Empty;
    }
}
