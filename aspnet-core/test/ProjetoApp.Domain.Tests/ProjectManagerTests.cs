using System;
using System.Threading.Tasks;
using NSubstitute;
using ProjetoApp.Domain;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Xunit;
using System.Linq.Expressions;

namespace ProjetoApp.Tests
{
    public class ProjectManagerTests
    {
        private readonly IRepository<Project, Guid> _mockProjectRepository;
        private readonly IRepository<ProjectTask, Guid> _mockProjectTaskRepository;
        private readonly ProjectTaskManager _projectTaskManager;
        private readonly ProjectManager _projectManager;

        public ProjectManagerTests()
        {
            // Configurar mocks para repositórios
            _mockProjectRepository = Substitute.For<IRepository<Project, Guid>>();
            _mockProjectTaskRepository = Substitute.For<IRepository<ProjectTask, Guid>>();
            
            // Criar instância real do ProjectTaskManager com repositórios mockados
            _projectTaskManager = new ProjectTaskManager(
                _mockProjectTaskRepository,
                Substitute.For<IRepository<Project, Guid>>(),
                Substitute.For<IRepository<TaskHistory, Guid>>(),
                Substitute.For<IRepository<TaskComment, Guid>>()
            );

            // Instanciar o ProjectManager com a instância real do ProjectTaskManager
            _projectManager = new ProjectManager(
                _mockProjectRepository,
                _projectTaskManager
            );
        }

        [Fact]
        public async Task Should_Delete_Project_When_No_Pending_Tasks()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            
            // Configurar o repositório mockado para retornar 0 tarefas pendentes
            // Isso fará com que ValidateProjectDeletionAsync não lance exceção
            _mockProjectTaskRepository
                .CountAsync(Arg.Any<Expression<Func<ProjectTask, bool>>>())
                .Returns(Task.FromResult(0));

            // Act
            await _projectManager.DeleteAsync(projectId);

            // Assert
            await _mockProjectRepository.Received(1).DeleteAsync(projectId);
        }

        [Fact]
        public async Task Should_Not_Delete_Project_With_Pending_Tasks()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            
            // Configurar o repositório mockado para retornar 1 tarefa pendente
            // Isso fará com que ValidateProjectDeletionAsync lance a exceção esperada
            _mockProjectTaskRepository
                .CountAsync(Arg.Any<Expression<Func<ProjectTask, bool>>>())
                .Returns(Task.FromResult(1));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await _projectManager.DeleteAsync(projectId);
            });

            exception.Code.ShouldBe("ProjectDomainErrorCodes.ProjectHasPendingTasks");
            
            // Garantir que o Delete não foi chamado
            await _mockProjectRepository.DidNotReceive().DeleteAsync(projectId);
        }
    }
}