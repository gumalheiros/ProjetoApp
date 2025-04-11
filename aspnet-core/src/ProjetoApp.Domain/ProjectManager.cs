using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ProjetoApp.Domain;

public class ProjectManager : DomainService
{
    private readonly IRepository<Project, Guid> _projectRepository;
    private readonly ProjectTaskManager _projectTaskManager;

    public ProjectManager(
        IRepository<Project, Guid> projectRepository,
        ProjectTaskManager projectTaskManager)
    {
        _projectRepository = projectRepository;
        _projectTaskManager = projectTaskManager;
    }

    public async Task DeleteAsync(Guid id)
    {
        // Valida se pode deletar o projeto
        await _projectTaskManager.ValidateProjectDeletionAsync(id);

        // Se chegou aqui, pode deletar
        await _projectRepository.DeleteAsync(id);
    }
}
