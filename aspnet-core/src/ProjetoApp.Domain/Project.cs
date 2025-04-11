using ProjetoApp.Domain.Shared;
using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace ProjetoApp.Domain;

public class Project : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid UserId { get; private set; }
    public virtual ICollection<ProjectTask> Tasks { get; private set; }

    private Project() { }

    internal Project(
        Guid id,
        string name, 
        string description,
        Guid userId) : base(id)
    {
        SetName(name);
        SetDescription(description);
        UserId = userId;
        Tasks = new List<ProjectTask>();
    }

    internal void SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), ProjectConsts.MaxNameLength);
    }

    internal void SetDescription(string description)
    {
        Description = Check.Length(description, nameof(description), ProjectConsts.MaxDescriptionLength);
    }
}
