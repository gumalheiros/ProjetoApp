using System;
using Volo.Abp.Domain.Entities;

namespace ProjetoApp.Domain;
public class Customer : Entity<Guid>
{
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }

    protected Customer()
    {
    }

    public Customer(
        Guid id,
        string name,
        DateTime birthDate
    ) : base(id)
    {
        Name = name;
        BirthDate = birthDate;
    }
}
