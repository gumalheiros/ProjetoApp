using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using ProjetoApp.Domain.Shared;

namespace ProjetoApp.Domain;

public class ProjectTask : FullAuditedEntity<Guid>
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime DueDate { get; private set; }
    public ProjectTaskStatus Status { get; private set; }
    public Guid ProjectId { get; private set; }
    public virtual Project Project { get; private set; }

    private ProjectTask() { }

    internal ProjectTask(
        Guid id,
        Guid projectId,
        string title,
        string description,
        DateTime dueDate) : base(id)
    {
        ProjectId = projectId;
        SetTitle(title);
        SetDescription(description);
        SetDueDate(dueDate);
        Status = ProjectTaskStatus.Pending;
    }

    internal void SetTitle(string title)
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title), ProjectTaskConsts.MaxTitleLength);
    }

    internal void SetDescription(string description)
    {
        Description = Check.Length(description, nameof(description), ProjectTaskConsts.MaxDescriptionLength);
    }

    internal void SetDueDate(DateTime dueDate)
    {
        DueDate = Check.NotNull(dueDate, nameof(dueDate));
    }

    internal void UpdateStatus(ProjectTaskStatus status)
    {
        Status = status;
    }
}
