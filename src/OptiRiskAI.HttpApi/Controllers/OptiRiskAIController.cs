using OptiRiskAI.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace OptiRiskAI.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class OptiRiskAIController : AbpControllerBase
{
    protected OptiRiskAIController()
    {
        LocalizationResource = typeof(OptiRiskAIResource);
    }
}
