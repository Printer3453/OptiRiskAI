using OptiRiskAI.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace OptiRiskAI.Web.Pages;

public abstract class OptiRiskAIPageModel : AbpPageModel
{
    protected OptiRiskAIPageModel()
    {
        LocalizationResourceType = typeof(OptiRiskAIResource);
    }
}
