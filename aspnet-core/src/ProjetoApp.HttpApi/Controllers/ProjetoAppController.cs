using ProjetoApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ProjetoApp.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ProjetoAppController : AbpControllerBase
{
    protected ProjetoAppController()
    {
        LocalizationResource = typeof(ProjetoAppResource);
    }
}
