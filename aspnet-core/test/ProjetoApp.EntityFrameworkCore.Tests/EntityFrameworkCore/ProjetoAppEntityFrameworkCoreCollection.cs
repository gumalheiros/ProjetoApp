using Xunit;

namespace ProjetoApp.EntityFrameworkCore;

[CollectionDefinition(ProjetoAppTestConsts.CollectionDefinitionName)]
public class ProjetoAppEntityFrameworkCoreCollection : ICollectionFixture<ProjetoAppEntityFrameworkCoreFixture>
{

}
