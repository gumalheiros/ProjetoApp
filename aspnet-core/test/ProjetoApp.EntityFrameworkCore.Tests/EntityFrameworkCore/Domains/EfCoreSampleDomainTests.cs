using ProjetoApp.Samples;
using Xunit;

namespace ProjetoApp.EntityFrameworkCore.Domains;

[Collection(ProjetoAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ProjetoAppEntityFrameworkCoreTestModule>
{

}
