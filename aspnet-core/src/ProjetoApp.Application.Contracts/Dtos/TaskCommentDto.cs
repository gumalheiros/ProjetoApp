
using System;
using Volo.Abp.Application.Dtos;

public class TaskCommentDto : CreationAuditedEntityDto<Guid>
{
    public string Content { get; set; }
}
