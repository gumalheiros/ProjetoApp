using ProjetoApp.Domain;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ProjetoApp.Projects;

public class ProjectTaskManager : DomainService
{
    private readonly IRepository<ProjectTask, Guid> _projectTaskRepository;
    private readonly IRepository<Project, Guid> _projectRepository;

    public ProjectTaskManager(
        IRepository<ProjectTask, Guid> projectTaskRepository,
        IRepository<Project, Guid> projectRepository)
    {
        _projectTaskRepository = projectTaskRepository;
        _projectRepository = projectRepository;
    }

    public async Task<ProjectTask> CreateAsync(
        Guid projectId,
        string title,
        string description,
        DateTime dueDate)
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

        // Criar a nova tarefa
        var task = new ProjectTask(
            GuidGenerator.Create(),
            projectId,
            title,
            description,
            dueDate
        );

        // Já começa com status Pending por padrão
        await _projectTaskRepository.InsertAsync(task);

        return task;
    }

    public async Task<ProjectTask> UpdateStatusAsync(Guid id, ProjectTaskStatus status)
    {
        var task = await _projectTaskRepository.GetAsync(id);

        ValidateStatusTransition(task.Status, status);

        task.UpdateStatus(status);

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
}
