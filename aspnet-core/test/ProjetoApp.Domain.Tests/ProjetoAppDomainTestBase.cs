using Volo.Abp.Modularity;

namespace ProjetoApp;

/* Inherit from this class for your domain layer tests. 
   This is a lightweight base class that doesn't depend on the full ABP initialization */
public abstract class ProjetoAppDomainTestBase
{
    // Esta classe não precisa herdar de nenhuma outra classe do ABP
    // Para testes de domínio puros, criamos mocks diretamente nos testes
}

/* Use this for tests that need ABP module initialization */
public abstract class ProjetoAppDomainTestBase<TStartupModule> : ProjetoAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
