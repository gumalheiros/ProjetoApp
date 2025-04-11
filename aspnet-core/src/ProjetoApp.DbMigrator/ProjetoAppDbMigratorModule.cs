using ProjetoApp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ProjetoApp.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ProjetoAppEntityFrameworkCoreModule),
    typeof(ProjetoAppApplicationContractsModule)
    )]
public class ProjetoAppDbMigratorModule : AbpModule
{
}
