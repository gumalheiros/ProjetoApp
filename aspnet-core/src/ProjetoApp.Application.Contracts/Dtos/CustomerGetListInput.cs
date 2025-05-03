using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace ProjetoApp.Domain.Dtos;

[Serializable]
public class CustomerGetListInput : PagedAndSortedResultRequestDto
{
    public string? Name { get; set; }

    public DateTime? BirthDate { get; set; }
}