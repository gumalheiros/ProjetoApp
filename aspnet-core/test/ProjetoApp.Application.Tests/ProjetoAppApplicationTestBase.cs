using Volo.Abp.Modularity;

namespace ProjetoApp;

public abstract class ProjetoAppApplicationTestBase<TStartupModule> : ProjetoAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
