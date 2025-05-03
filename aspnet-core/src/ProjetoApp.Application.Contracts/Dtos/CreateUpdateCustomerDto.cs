using System;

namespace ProjetoApp.Domain.Dtos;

[Serializable]
public class CreateUpdateCustomerDto
{
    public string Name { get; set; }

    public DateTime BirthDate { get; set; }
}