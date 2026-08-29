using OptiRiskAI.Localization;
using Volo.Abp.Application.Services;

namespace OptiRiskAI;

/* Inherit your application services from this class.
 */
public abstract class OptiRiskAIAppService : ApplicationService
{
    protected OptiRiskAIAppService()
    {
        LocalizationResource = typeof(OptiRiskAIResource);
    }
}
