using Volo.Abp.Modularity;

namespace ProjetoApp;

/* Inherit from this class for your domain layer tests. */
public abstract class ProjetoAppDomainTestBase<TStartupModule> : ProjetoAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
