using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OptiRiskAI.RiskIntelligence
{
    public class GenerateRuleFromPromptDto
    {
        [Required]
        [MaxLength(1000)]
        public string Prompt { get; set; } = string.Empty;
    }
}
