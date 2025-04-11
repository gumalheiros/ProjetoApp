using System.Threading.Tasks;

namespace ProjetoApp.Data;

public interface IProjetoAppDbSchemaMigrator
{
    Task MigrateAsync();
}
