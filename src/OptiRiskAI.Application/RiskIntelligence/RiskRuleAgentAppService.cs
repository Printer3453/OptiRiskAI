using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OptiRiskAI.RiskIntelligence
{
    public class RiskRuleAgentAppService : ApplicationService, IRiskRuleAgentAppService
    {
        private readonly IRepository<RiskRule, Guid> _ruleRepository;
        private readonly Kernel _kernel;

        public RiskRuleAgentAppService(IRepository<RiskRule, Guid> ruleRepository)
        {
            _ruleRepository = ruleRepository;

            // Agentic Workflow: Senin yerel Container bazlı AI modeline bağlantı
            var builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion(
                modelId: "llama3.1",
                apiKey: "yerel-icin-gerek-yok",
                endpoint: new Uri("http://localhost:11434/v1") // Yerel LLM portun
            );

            _kernel = builder.Build();
        }

        
        public async Task<RiskRuleDto> GenerateRiskRuleFromPromptAsync(GenerateRuleFromPromptDto input)
        {
            
            var prompt = $@"
Sen endüstriyel risk analiz kuralları üreten otonom bir mimarsın. 
Kullanıcının verdiği metni analiz et ve sistemimizin okuyabileceği yapısal bir kurala (JSON) dönüştür.

Kullanıcı Komutu: ""{input.Prompt}""

Sadece aşağıdaki JSON formatında bir çıktı üret. Başka hiçbir açıklama, yorum veya kod bloğu işareti ekleme:
{{
    ""ruleName"": ""(Örn: High Wind Pine Forest Risk)"",
    ""conditionExpression"": ""(Örn: WindSpeedKmh > 60 AND VegetationType == 'Pine Forest')"",
    ""riskMultiplier"": (Ondalıklı risk çarpanı değeri, örn: 3.5),
    ""aiRationale"": ""(Bu kuralı neden ürettiğine dair kısa bir mühendislik gerekçesi, Türkçe veya İngilizce)""
}}";

            var result = await _kernel.InvokePromptAsync(prompt);
            var aiResponse = result.GetValue<string>();

            
            var aiDecision = JsonSerializer.Deserialize<AiRuleDecisionModel>(aiResponse);

            if (aiDecision == null || string.IsNullOrWhiteSpace(aiDecision.ruleName))
            {
                throw new Exception("Yapay zeka ajanı bu komuttan geçerli bir kural üretemedi.");
            }

            
            var newRule = new RiskRule(
                GuidGenerator.Create(),
                aiDecision.ruleName,
                aiDecision.conditionExpression,
                aiDecision.riskMultiplier,
                aiDecision.aiRationale
            );

            await _ruleRepository.InsertAsync(newRule);

            
            return new RiskRuleDto
            {
                Id = newRule.Id,
                RuleName = newRule.RuleName,
                ConditionExpression = newRule.ConditionExpression,
                RiskMultiplier = newRule.RiskMultiplier,
                AiRationale = newRule.AiRationale,
                IsActive = newRule.IsActive
            };
        }
    }

    
    public class AiRuleDecisionModel
    {
        public string ruleName { get; set; } = string.Empty;
        public string conditionExpression { get; set; } = string.Empty;
        public decimal riskMultiplier { get; set; }
        public string aiRationale { get; set; } = string.Empty;
    }
}