using System;
using System.Linq;
using System.Threading.Tasks;
using ProjetoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ProjetoApp.Domain;

public class CustomerRepository : EfCoreRepository<ProjetoAppDbContext, Customer, Guid>, ICustomerRepository
{
    public CustomerRepository(IDbContextProvider<ProjetoAppDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<Customer>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}