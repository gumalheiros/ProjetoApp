using System;
using System.Threading.Tasks;
using ProjetoApp.Projects.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ProjetoApp.Projects;

public interface IProjectAppService : 
    ICrudAppService<
        ProjectDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateProjectDto,
        CreateUpdateProjectDto>
{
    Task<PagedResultDto<ProjectDto>> GetMyProjectsAsync(PagedAndSortedResultRequestDto input);
}
