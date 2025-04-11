using Volo.Abp.Modularity;

namespace ProjetoApp;

[DependsOn(
    typeof(ProjetoAppApplicationModule),
    typeof(ProjetoAppDomainTestModule)
)]
public class ProjetoAppApplicationTestModule : AbpModule
{

}
