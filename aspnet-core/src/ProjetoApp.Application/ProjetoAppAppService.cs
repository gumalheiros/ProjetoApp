using System;
using System.Collections.Generic;
using System.Text;
using ProjetoApp.Localization;
using Volo.Abp.Application.Services;

namespace ProjetoApp;

/* Inherit your application services from this class.
 */
public abstract class ProjetoAppAppService : ApplicationService
{
    protected ProjetoAppAppService()
    {
        LocalizationResource = typeof(ProjetoAppResource);
    }
}
