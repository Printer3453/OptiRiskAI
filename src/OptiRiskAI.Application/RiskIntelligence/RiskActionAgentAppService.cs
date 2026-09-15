using System;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Volo.Abp.Application.Services;

namespace OptiRiskAI.RiskIntelligence
{
    public class RiskActionAgentAppService : ApplicationService, IRiskActionAgentAppService
    {
        private readonly Kernel _kernel;

        public RiskActionAgentAppService()
        {
            var builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion(
                modelId: "llama3.1", 
                apiKey: "yerel-icin-gerek-yok", 
                endpoint: new Uri("http://localhost:11434/v1") 
            );
            
            _kernel = builder.Build();
        }

        public async Task<RiskActionDecisionDto> EvaluateRiskScoreAndDecideActionAsync(decimal calculatedScore, double latitude, double longitude)
        {
            // 1. Zırhlı Karar Mekanizması: Kararı LLM değil, C# veriyor.
            string determinedAction = "";
            string determinedDepartment = "";

            if (calculatedScore > 80)
            {
                determinedAction = "BLOCK_POLICY";
                determinedDepartment = "Underwriting";
            }
            else if (calculatedScore >= 50)
            {
                determinedAction = "ALERT_TEAM";
                determinedDepartment = "Maintenance";
            }
            else
            {
                determinedAction = "LOG_ONLY";
                determinedDepartment = "System";
            }

            // 2. Operasyonel Ajan (Semantic Kernel): Sadece iletişimi ve gerekçeyi (Reasoning) yazıyor.
            var prompt = $@"
Sen bir Climate-FinTech şirketinde çalışan otonom risk operasyon ajanısın.
Sistem, {latitude}, {longitude} lokasyonu için {calculatedScore}/100 Ateşleme Olasılığı (İklim Risk Skoru) hesapladı.
Bu skor doğrultusunda '{determinedDepartment}' departmanına '{determinedAction}' iş emri gönderilmesine kesin karar verildi.

Görev: Bu aksiyonun neden alındığını açıklayan, ilgili departmanın okuyacağı tek bir cümlelik profesyonel bir gerekçe (AiReasoning) yaz.
ÇOK ÖNEMLİ KURAL: Eğer karar 'BLOCK_POLICY' ise, kuracağın cümlede mutlaka 'Loss Ratio (Hasar Oranı) dengesi' ve 'ZAS portföy rezervlerinin korunması' ifadelerini kullanmalısın.

Sadece gerekçe cümlesini döndür, JSON formatı veya ek bir açıklama kullanma.";

            var result = await _kernel.InvokePromptAsync(prompt);
            var aiReasoningText = result.GetValue<string>()?.Trim();

            // 3. Kesin karar ve AI'ın yorumu birleşiyor
            return new RiskActionDecisionDto
            {
                ActionType = determinedAction,
                TargetDepartment = determinedDepartment,
                AiReasoning = string.IsNullOrWhiteSpace(aiReasoningText) ? "Risk skoru eşik değerleri aştığı için otomatik aksiyon alındı." : aiReasoningText
            };
        }
    }
}