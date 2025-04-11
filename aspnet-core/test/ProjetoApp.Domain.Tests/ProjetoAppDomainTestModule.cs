using Volo.Abp.Modularity;

namespace ProjetoApp;

[DependsOn(
    typeof(ProjetoAppDomainModule),
    typeof(ProjetoAppTestBaseModule)
)]
public class ProjetoAppDomainTestModule : AbpModule
{

}
