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

    public ProjectTaskManager(IRepository<ProjectTask, Guid> projectTaskRepository)
    {
        _projectTaskRepository = projectTaskRepository;
    }

    public async Task<ProjectTask> UpdateStatusAsync(Guid id, ProjectTaskStatus status)
    {
        var task = await _projectTaskRepository.GetAsync(id);
        
        // Aqui podemos adicionar regras de negócio específicas do domínio
        // Por exemplo, validar transições de status permitidas
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
