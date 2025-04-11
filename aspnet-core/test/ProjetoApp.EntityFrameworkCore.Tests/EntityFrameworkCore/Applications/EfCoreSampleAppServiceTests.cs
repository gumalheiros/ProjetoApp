using ProjetoApp.Samples;
using Xunit;

namespace ProjetoApp.EntityFrameworkCore.Applications;

[Collection(ProjetoAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ProjetoAppEntityFrameworkCoreTestModule>
{

}
