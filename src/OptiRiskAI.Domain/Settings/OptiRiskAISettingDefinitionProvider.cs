using Volo.Abp.Settings;

namespace OptiRiskAI.Settings;

public class OptiRiskAISettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(OptiRiskAISettings.MySetting1));
    }
}
