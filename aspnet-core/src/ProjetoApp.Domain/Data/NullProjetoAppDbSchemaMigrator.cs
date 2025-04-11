using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ProjetoApp.Data;

/* This is used if database provider does't define
 * IProjetoAppDbSchemaMigrator implementation.
 */
public class NullProjetoAppDbSchemaMigrator : IProjetoAppDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
