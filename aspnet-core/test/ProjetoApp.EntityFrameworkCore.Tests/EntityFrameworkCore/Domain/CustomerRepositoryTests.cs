using System;
using System.Threading.Tasks;
using ProjetoApp.Domain;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace ProjetoApp.EntityFrameworkCore.Domain;

public class CustomerRepositoryTests : ProjetoAppEntityFrameworkCoreTestBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerRepositoryTests()
    {
        _customerRepository = GetRequiredService<ICustomerRepository>();
    }

    /*
    [Fact]
    public async Task Test1()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            // Arrange

            // Act

            //Assert
        });
    }
    */
}
