using System;
using System.Threading.Tasks;
using ProjetoApp.Domain;
using ProjetoApp.Projects.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ProjetoApp.Projects;

public interface IProjectTaskAppService :
    ICrudAppService<
        ProjectTaskDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateProjectTaskDto,
        CreateUpdateProjectTaskDto>
{
    Task<ProjectTaskDto> UpdateStatusAsync(Guid id, ProjectTaskStatus status);
    Task<PagedResultDto<ProjectTaskDto>> GetProjectTasksAsync(Guid projectId, PagedAndSortedResultRequestDto input);
}
