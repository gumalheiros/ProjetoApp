using Microsoft.Extensions.Localization;
using ProjetoApp.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ProjetoApp;

[Dependency(ReplaceServices = true)]
public class ProjetoAppBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ProjetoAppResource> _localizer;

    public ProjetoAppBrandingProvider(IStringLocalizer<ProjetoAppResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
