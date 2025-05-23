using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProjetoApp.Domain;
using ProjetoApp.Dtos;
using ProjetoApp.Permissions;
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
    private readonly IRepository<TaskHistory, Guid> _taskHistoryRepository;
    private readonly IRepository<TaskComment, Guid> _taskCommentRepository;
    private readonly ProjectTaskReportManager _reportManager;

    public ProjectTaskAppService(
        IRepository<ProjectTask, Guid> repository,
        IRepository<Project, Guid> projectRepository,
        ProjectTaskManager projectTaskManager,
        IRepository<TaskHistory, Guid> taskHistoryRepository,
        IRepository<TaskComment, Guid> taskCommentRepository,
        ProjectTaskReportManager reportManager)
        : base(repository)
    {
        _projectRepository = projectRepository;
        _projectTaskManager = projectTaskManager;
        _taskHistoryRepository = taskHistoryRepository;
        _taskCommentRepository = taskCommentRepository;
        _reportManager = reportManager;
    }

    public async Task<ProjectTaskDto> UpdateStatusAsync(Guid id, ProjectTaskStatus status)
    {
        await CheckUpdatePolicyAsync();

        var task = await _projectTaskManager.UpdateStatusAsync(id, status, CurrentUser.Id.Value);
        
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

    protected override async Task<ProjectTask> MapToEntityAsync(CreateUpdateProjectTaskDto createInput)
    {
        // Usa o ProjectTaskManager para criar a entidade
        return await _projectTaskManager.CreateAsync(
            createInput.ProjectId,
            createInput.Title,
            createInput.Description,
            createInput.DueDate,
            createInput.Priority
        );
    }

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

    public async Task<List<TaskHistoryDto>> GetHistoryAsync(Guid taskId)
    {
        var history = await _taskHistoryRepository
            .GetListAsync(h => h.TaskId == taskId);

        history = history.OrderByDescending(h => h.ChangedAt).ToList();

        return ObjectMapper.Map<List<TaskHistory>, List<TaskHistoryDto>>(history);
    }

    public async Task<TaskCommentDto> AddCommentAsync(Guid taskId, CreateTaskCommentDto input)
    {
        var comment = await _projectTaskManager.AddCommentAsync(
            taskId,
            input.Content,
            CurrentUser.Id.Value
        );

        return ObjectMapper.Map<TaskComment, TaskCommentDto>(comment);
    }

    public async Task<List<TaskCommentDto>> GetCommentsAsync(Guid taskId)
    {
        var comments = await _taskCommentRepository
            .GetListAsync(c => c.TaskId == taskId);

        comments = comments.OrderByDescending(c => c.CreationTime).ToList();

        return ObjectMapper.Map<List<TaskComment>, List<TaskCommentDto>>(comments);
    }

    [Authorize(ProjetoAppPermissions.Projects.ViewReports)]
    public async Task<List<TaskPerformanceReportDto>> GetPerformanceReportAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        // Define per�odo padr�o de 30 dias se n�o especificado
        var end = endDate ?? Clock.Now;
        var start = startDate ?? end.AddDays(-30);

        var report = await _reportManager.GenerateUserPerformanceReportAsync(start, end);

        return ObjectMapper.Map<List<TaskPerformanceReport>, List<TaskPerformanceReportDto>>(report);
    }

}
