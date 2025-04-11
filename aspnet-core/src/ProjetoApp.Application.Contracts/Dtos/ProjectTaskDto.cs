using ProjetoApp.Domain;
using System;
using Volo.Abp.Application.Dtos;

namespace ProjetoApp.Projects.Dtos;

public class ProjectTaskDto : AuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public Guid ProjectId { get; set; }
    public TaskPriority Priority { get; set; }
}
