using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ProjetoApp.Domain;

public class ProjectTaskManager : DomainService
{
    private const int MaxTasksPerProject = 20;
    private readonly IRepository<ProjectTask, Guid> _projectTaskRepository;
    private readonly IRepository<Project, Guid> _projectRepository;
    private readonly IRepository<TaskHistory, Guid> _taskHistoryRepository;
    private readonly IRepository<TaskComment, Guid> _taskCommentRepository;

    public ProjectTaskManager(
        IRepository<ProjectTask, Guid> projectTaskRepository,
        IRepository<Project, Guid> projectRepository,
        IRepository<TaskHistory, Guid> taskHistoryRepository,
        IRepository<TaskComment, Guid> taskCommentRepository
        )
    {
        _projectTaskRepository = projectTaskRepository;
        _projectRepository = projectRepository;
        _taskHistoryRepository = taskHistoryRepository;
        _taskCommentRepository = taskCommentRepository;
    }

    public async Task<ProjectTask> CreateAsync(
        Guid projectId,
        string title,
        string description,
        DateTime dueDate,
        TaskPriority priority)
    {
        // Validar se o projeto existe
        var project = await _projectRepository.GetAsync(projectId);
        if (project == null)
        {
            throw new BusinessException(
                "ProjectTaskDomainErrorCodes.ProjectNotFound",
                "Project not found"
            );
        }

        // Validar a data de vencimento
        if (dueDate < Clock.Now)
        {
            throw new BusinessException(
                "ProjectTaskDomainErrorCodes.InvalidDueDate",
                "Due date cannot be in the past"
            );
        }

        // Validar limite de tarefas
        var taskCount = await _projectTaskRepository.CountAsync(t => t.ProjectId == projectId);
        if (taskCount >= MaxTasksPerProject)
        {
            throw new BusinessException(
                "ProjectTaskDomainErrorCodes.TaskLimitExceeded",
                $"Project has reached the maximum limit of {MaxTasksPerProject} tasks"
            );
        }

        // Criar a nova tarefa
        var task = new ProjectTask(
            GuidGenerator.Create(),
            projectId,
            title,
            description,
            dueDate,
            priority
        );

        // Já começa com status Pending por padrão
        await _projectTaskRepository.InsertAsync(task);

        return task;
    }

    public async Task<ProjectTask> UpdateStatusAsync(Guid id, ProjectTaskStatus status, Guid userId)
    {
        var task = await _projectTaskRepository.GetAsync(id);

        var oldStatus = task.Status;
        ValidateStatusTransition(oldStatus, status);

        task.UpdateStatus(status);

        // Registrar a mudança no histórico
        await AddHistoryEntryAsync(
            id,
            "Status",
            oldStatus.ToString(),
            status.ToString(),
            userId,
            "Update"
        );

        return task;
    }

    private void ValidateStatusTransition(ProjectTaskStatus currentStatus, ProjectTaskStatus newStatus)
    {
        // Exemplo de regra de negócio: não pode voltar para Pending se já estiver Completed
        if (currentStatus == ProjectTaskStatus.Completed && newStatus == ProjectTaskStatus.Pending)
        {
            throw new BusinessException(
                "ProjectTaskDomainErrorCodes.InvalidStatusTransition",
                "Cannot change status from Completed to Pending"
            );
        }
    }

    public async Task ValidateProjectDeletionAsync(Guid projectId)
    {
        var pendingTasks = await _projectTaskRepository.CountAsync(
            task => task.ProjectId == projectId &&
                   task.Status != ProjectTaskStatus.Completed
        );

        if (pendingTasks > 0)
        {
            throw new BusinessException(
                "ProjectDomainErrorCodes.ProjectHasPendingTasks",
                "Cannot delete project with pending tasks. Please complete or remove all tasks first."
            );
        }
    }

    public async Task<TaskComment> AddCommentAsync(Guid taskId, string content, Guid userId)
    {
        var task = await _projectTaskRepository.GetAsync(taskId);

        var comment = new TaskComment(
            GuidGenerator.Create(),
            taskId,
            content
        );

        await _taskCommentRepository.InsertAsync(comment);

        // Registrar o comentário no histórico
        await AddHistoryEntryAsync(
            taskId,
            "Comment",
            null,
            content,
            userId,
            "Comment"
        );

        return comment;
    }

    private async Task AddHistoryEntryAsync(
        Guid taskId,
        string fieldName,
        string oldValue,
        string newValue,
        Guid userId,
        string changeType)
    {
        var history = new TaskHistory(
            GuidGenerator.Create(),
            taskId,
            fieldName,
            oldValue,
            newValue,
            userId,
            changeType
        );

        await _taskHistoryRepository.InsertAsync(history);
    }

}
