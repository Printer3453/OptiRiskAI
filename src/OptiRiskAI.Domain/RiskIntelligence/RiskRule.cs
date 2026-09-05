using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;


namespace OptiRiskAI.RiskIntelligence
{
    public class RiskRule : FullAuditedAggregateRoot<Guid>
    {
        public string RuleName { get; private set; }

        // Yapay zekanın ürettiği koşul (Örn: "Mesafe < 10 VE Ruzgar > 40")
        public string ConditionExpression { get; private set; }

        // Bu kural eşleştiğinde taban prime uygulanacak çarpan (Örn: 2.5)
        public decimal RiskMultiplier { get; private set; }

        // Yapay zekanın bu kuralı neden ürettiğine dair gerekçesi
        public string AiRationale { get; private set; }

        public bool IsActive { get; private set; }

        // Entity Framework Core'un reflection yapabilmesi için boş constructor (DDD kuralı)
        private RiskRule()
        {
        }

        public RiskRule
            (
            Guid id,
            string ruleName, 
            string conditionExpression, 
            decimal riskMultiplier, 
            string aiRationale
            ) 
        {
            RuleName = ruleName;
            ConditionExpression = conditionExpression;
            RiskMultiplier = riskMultiplier;
            AiRationale = aiRationale;
            IsActive = true;
        }

       

        // Kuralın çarpanını veya gerekçesini güncellemek için davranışsal metod 
        public void UpdateRule(decimal newMultiplier, string newRationale)
        {
            RiskMultiplier = newMultiplier;
            AiRationale = newRationale;
        }

       
        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }




    }
}
