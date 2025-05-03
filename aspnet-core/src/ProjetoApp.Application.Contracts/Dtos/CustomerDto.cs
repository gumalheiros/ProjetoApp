using System;
using Volo.Abp.Application.Dtos;

namespace ProjetoApp.Domain.Dtos;

[Serializable]
public class CustomerDto : EntityDto<Guid>
{
    public string Name { get; set; }

    public DateTime BirthDate { get; set; }
}