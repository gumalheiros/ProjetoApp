// src/ProjetoApp.Domain/TaskHistory.cs
using System;
using Volo.Abp.Domain.Entities;

namespace ProjetoApp.Domain;

public class TaskHistory : Entity<Guid>
{
    public Guid TaskId { get; private set; }
    public virtual ProjectTask Task { get; private set; }
    public string FieldName { get; private set; }
    public string OldValue { get; private set; }
    public string NewValue { get; private set; }
    public DateTime ChangedAt { get; private set; }
    public Guid ChangedByUserId { get; private set; }
    public string ChangeType { get; private set; } // Update, Comment, etc.

    private TaskHistory() { }

    public TaskHistory(
        Guid id,
        Guid taskId,
        string fieldName,
        string oldValue,
        string newValue,
        Guid changedByUserId,
        string changeType) : base(id)
    {
        TaskId = taskId;
        FieldName = fieldName;
        OldValue = oldValue;
        NewValue = newValue;
        ChangedAt = DateTime.UtcNow;
        ChangedByUserId = changedByUserId;
        ChangeType = changeType;
    }
}
