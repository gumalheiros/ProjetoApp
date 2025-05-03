using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace ProjetoApp.Application.Tests.Domain;

public class CustomerAppServiceTests : ProjetoAppApplicationTestBase<ProjetoAppApplicationTestModule>
{
    private readonly ICustomerAppService _customerAppService;

    public CustomerAppServiceTests()
    {
        _customerAppService = GetRequiredService<ICustomerAppService>();
    }

    /*
    [Fact]
    public async Task Test1()
    {
        // Arrange

        // Act

        // Assert
    }
    */
}

