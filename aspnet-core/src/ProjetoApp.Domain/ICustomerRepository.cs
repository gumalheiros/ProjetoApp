using System;
using Volo.Abp.Domain.Repositories;

namespace ProjetoApp.Domain;

public interface ICustomerRepository : IRepository<Customer, Guid>
{
}
