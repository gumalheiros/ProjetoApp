using System;
using Volo.Abp.Application.Dtos;

public class TaskHistoryDto : EntityDto<Guid>
{
    public string FieldName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public DateTime ChangedAt { get; set; }
    public Guid ChangedByUserId { get; set; }
    public string ChangeType { get; set; }
}