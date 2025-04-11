using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace ProjetoApp.Projects.Dtos;

public class ProjectDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ProjectTaskDto> Tasks { get; set; }
}
