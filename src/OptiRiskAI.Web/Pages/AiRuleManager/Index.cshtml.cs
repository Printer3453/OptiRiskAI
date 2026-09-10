using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OptiRiskAI.RiskIntelligence;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace OptiRiskAI.Web.Pages.AiRuleManager
{
    public class IndexModel : AbpPageModel
    {
        private readonly IRiskRuleAgentAppService _riskRuleAgentAppService;

        [BindProperty]
        public GenerateRuleFromPromptDto Input { get; set; }

        public RiskRuleDto GeneratedRule { get; set; }//

        // Yapay zeka ajanımızı arayüze (UI) bağlıyoruz
        public IndexModel(IRiskRuleAgentAppService riskRuleAgentAppService)
        {
            _riskRuleAgentAppService = riskRuleAgentAppService;
        }

        public void OnGet()
        {
            Input = new GenerateRuleFromPromptDto();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Butona basıldığında formdaki metni alıp Ollama'ya gönderiyoruz
            GeneratedRule = await _riskRuleAgentAppService.GenerateRiskRuleFromPromptAsync(Input);

            return Page();
        }
    }
}