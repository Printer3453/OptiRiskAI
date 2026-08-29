using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using OptiRiskAI.Localization;

namespace OptiRiskAI.Web;

[Dependency(ReplaceServices = true)]
public class OptiRiskAIBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<OptiRiskAIResource> _localizer;

    public OptiRiskAIBrandingProvider(IStringLocalizer<OptiRiskAIResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
