using Volo.Abp.Settings;

namespace ProjetoApp.Settings;

public class ProjetoAppSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ProjetoAppSettings.MySetting1));
    }
}
