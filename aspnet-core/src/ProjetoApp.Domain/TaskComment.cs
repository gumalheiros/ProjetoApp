// src/ProjetoApp.Domain/TaskComment.cs
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ProjetoApp.Domain;

public class TaskComment : CreationAuditedEntity<Guid>
{
    public Guid TaskId { get; private set; }
    public virtual ProjectTask Task { get; private set; }
    public string Content { get; private set; }

    private TaskComment() { }

    public TaskComment(
        Guid id,
        Guid taskId,
        string content) : base(id)
    {
        TaskId = taskId;
        Content = content;
    }
}
