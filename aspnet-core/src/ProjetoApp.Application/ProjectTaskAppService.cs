using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProjetoApp.Domain;
using ProjetoApp.Projects.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ProjetoApp.Projects;

public class ProjectTaskAppService :
    CrudAppService<
        ProjectTask,
        ProjectTaskDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateProjectTaskDto,
        CreateUpdateProjectTaskDto>,
    IProjectTaskAppService
{
    private readonly IRepository<Project, Guid> _projectRepository;
    private readonly ProjectTaskManager _projectTaskManager;

    public ProjectTaskAppService(
        IRepository<ProjectTask, Guid> repository,
        IRepository<Project, Guid> projectRepository,
        ProjectTaskManager projectTaskManager)
        : base(repository)
    {
        _projectRepository = projectRepository;
        _projectTaskManager = projectTaskManager;

    }

    public async Task<ProjectTaskDto> UpdateStatusAsync(Guid id, ProjectTaskStatus status)
    {
        await CheckUpdatePolicyAsync();

        var task = await _projectTaskManager.UpdateStatusAsync(id, status);
        
        await Repository.UpdateAsync(task);

        return ObjectMapper.Map<ProjectTask, ProjectTaskDto>(task);
    }

    public async Task<PagedResultDto<ProjectTaskDto>> GetProjectTasksAsync(
        Guid projectId,
        PagedAndSortedResultRequestDto input)
    {
        await CheckGetListPolicyAsync();

        // Verifica se o projeto existe
        var project = await _projectRepository.GetAsync(projectId);

        var query = await Repository.GetQueryableAsync();
        query = query
            .Where(x => x.ProjectId == projectId);

        // Apply sorting if provided
        if (!string.IsNullOrWhiteSpace(input.Sorting))
        {
            query = query.OrderBy(input.Sorting);
        }
        else
        {
            query = ApplyDefaultSorting(query);
        }

        // Apply pagination
        var totalCount = await AsyncExecuter.CountAsync(query);
        var items = await AsyncExecuter.ToListAsync(
            query.Skip(input.SkipCount).Take(input.MaxResultCount)
        );

        var dtos = ObjectMapper.Map<List<ProjectTask>, List<ProjectTaskDto>>(items);

        return new PagedResultDto<ProjectTaskDto>(totalCount, dtos);
    }

    //protected override async Task<ProjectTask> MapToEntityAsync(CreateUpdateProjectTaskDto createInput)
    //{
    //    // Verifica se o projeto existe
    //    var project = await _projectRepository.GetAsync(createInput.ProjectId);

    //    return new ProjectTask(
    //        GuidGenerator.Create(),
    //        createInput.ProjectId,
    //        createInput.Title,
    //        createInput.Description,
    //        createInput.DueDate
    //    );
    //}

    protected override async Task<ProjectTask> GetEntityByIdAsync(Guid id)
    {
        // Use WithDetailsAsync instead of Include to fetch related entities
        var queryable = await Repository.WithDetailsAsync(x => x.Project);
        return await queryable.FirstOrDefaultAsync(x => x.Id == id);
    }

    protected override IQueryable<ProjectTask> ApplyDefaultSorting(IQueryable<ProjectTask> query)
    {
        return query.OrderByDescending(x => x.DueDate);
    }
}
