using System;
using ProjetoApp.Domain.Dtos;
using Volo.Abp.Application.Services;

namespace ProjetoApp;


public interface ICustomerAppService :
    ICrudAppService< 
        CustomerDto, 
        Guid, 
        CustomerGetListInput,
        CreateUpdateCustomerDto,
        CreateUpdateCustomerDto>
{

}