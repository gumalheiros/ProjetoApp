using System;
using System.Linq;
using System.Threading.Tasks;
using ProjetoApp.Permissions;
using ProjetoApp.Domain.Dtos;
using Volo.Abp.Application.Services;

namespace ProjetoApp.Domain;


public class CustomerAppService : CrudAppService<Customer, CustomerDto, Guid, CustomerGetListInput, CreateUpdateCustomerDto, CreateUpdateCustomerDto>,
    ICustomerAppService
{
    protected override string GetPolicyName { get; set; } = ProjetoAppPermissions.Customer.Default;
    protected override string GetListPolicyName { get; set; } = ProjetoAppPermissions.Customer.Default;
    protected override string CreatePolicyName { get; set; } = ProjetoAppPermissions.Customer.Create;
    protected override string UpdatePolicyName { get; set; } = ProjetoAppPermissions.Customer.Update;
    protected override string DeletePolicyName { get; set; } = ProjetoAppPermissions.Customer.Delete;

    private readonly ICustomerRepository _repository;

    public CustomerAppService(ICustomerRepository repository) : base(repository)
    {
        _repository = repository;
    }

    protected override async Task<IQueryable<Customer>> CreateFilteredQueryAsync(CustomerGetListInput input)
    {
        // TODO: AbpHelper generated
        return (await base.CreateFilteredQueryAsync(input))
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            .WhereIf(input.BirthDate != null, x => x.BirthDate == input.BirthDate)
            ;
    }
}
