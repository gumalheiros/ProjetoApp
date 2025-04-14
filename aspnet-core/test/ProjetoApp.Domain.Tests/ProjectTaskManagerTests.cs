using System;
using System.Threading.Tasks;
using NSubstitute;
using ProjetoApp.Domain;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Timing;
using Xunit;

namespace ProjetoApp.Tests
{
    public class ProjectTaskManagerTests
    {
        private readonly IRepository<ProjectTask, Guid> _mockProjectTaskRepository;
        private readonly IRepository<Project, Guid> _mockProjectRepository;
        private readonly IRepository<TaskHistory, Guid> _mockTaskHistoryRepository;
        private readonly IRepository<TaskComment, Guid> _mockTaskCommentRepository;
        private readonly ProjectTaskManager _projectTaskManager;

        public ProjectTaskManagerTests()
        {
            // Configurar mocks para os repositórios
            _mockProjectTaskRepository = Substitute.For<IRepository<ProjectTask, Guid>>();
            _mockProjectRepository = Substitute.For<IRepository<Project, Guid>>();
            _mockTaskHistoryRepository = Substitute.For<IRepository<TaskHistory, Guid>>();
            _mockTaskCommentRepository = Substitute.For<IRepository<TaskComment, Guid>>();
            
           // Instanciar diretamente o ProjectTaskManager com os mocks
            _projectTaskManager = new ProjectTaskManager(
                _mockProjectTaskRepository,
                _mockProjectRepository,
                _mockTaskHistoryRepository,
                _mockTaskCommentRepository
            );
        }

        [Fact]
        public async Task Should_Create_Task_With_Priority()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project(projectId, "Test Project", "Test Description", Guid.NewGuid());
            
            _mockProjectRepository.GetAsync(projectId).Returns(project);
            _mockProjectTaskRepository.CountAsync(Arg.Any<System.Linq.Expressions.Expression<Func<ProjectTask, bool>>>())
                .Returns(0); // Sem tarefas no projeto ainda

            // Act
            var taskPriority = TaskPriority.High;
            var result = await _projectTaskManager.CreateAsync(
                projectId,
                "Test Task",
                "Test Description",
                DateTime.Now.AddDays(1),
                taskPriority
            );

            // Assert
            result.ShouldNotBeNull();
            result.Priority.ShouldBe(taskPriority);
        }

        [Fact]
        public async Task Should_Not_Exceed_Task_Limit_Per_Project()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project(projectId, "Test Project", "Test Description", Guid.NewGuid());
            
            _mockProjectRepository.GetAsync(projectId).Returns(project);
            _mockProjectTaskRepository.CountAsync(Arg.Any<System.Linq.Expressions.Expression<Func<ProjectTask, bool>>>())
                .Returns(20); // Já tem 20 tarefas (limite máximo)

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await _projectTaskManager.CreateAsync(
                    projectId,
                    "Test Task",
                    "Test Description",
                    DateTime.Now.AddDays(1),
                    TaskPriority.Medium
                );
            });

            exception.Code.ShouldContain("TaskLimitExceeded");
        }

        [Fact]
        public async Task Should_Add_History_Entry_When_Status_Updated()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var task = new ProjectTask(
                taskId,
                Guid.NewGuid(),
                "Test Task",
                "Description",
                DateTime.Now.AddDays(1),
                TaskPriority.Medium
            );
            
            _mockProjectTaskRepository.GetAsync(taskId).Returns(task);

            // Act
            await _projectTaskManager.UpdateStatusAsync(taskId, ProjectTaskStatus.InProgress, userId);

            // Assert
            await _mockTaskHistoryRepository.Received(1).InsertAsync(
                Arg.Is<TaskHistory>(h => 
                    h.TaskId == taskId && 
                    h.FieldName == "Status" && 
                    h.OldValue == ProjectTaskStatus.Pending.ToString() && 
                    h.NewValue == ProjectTaskStatus.InProgress.ToString() &&
                    h.ChangedByUserId == userId
                )
            );
        }

        [Fact]
        public async Task Should_Validate_Project_For_Deletion()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            
            // Configurar para ter tarefas pendentes
            _mockProjectTaskRepository.CountAsync(
                Arg.Any<System.Linq.Expressions.Expression<Func<ProjectTask, bool>>>()
            ).Returns(5); // 5 tarefas pendentes

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
            {
                await _projectTaskManager.ValidateProjectDeletionAsync(projectId);
            });

            exception.Code.ShouldContain("ProjectHasPendingTasks");
        }

        [Fact]
        public async Task Should_Allow_Project_Deletion_When_No_Pending_Tasks()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            
            // Configurar para não ter tarefas pendentes
            _mockProjectTaskRepository.CountAsync(
                Arg.Any<System.Linq.Expressions.Expression<Func<ProjectTask, bool>>>()
            ).Returns(0);

            // Act & Assert - Não deve lançar exceção
            await _projectTaskManager.ValidateProjectDeletionAsync(projectId);
        }

        [Fact]
        public async Task Should_Register_Comment_And_History()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var content = "Comentário de teste";
            
            var task = new ProjectTask(
                taskId,
                Guid.NewGuid(),
                "Test Task",
                "Description",
                DateTime.Now.AddDays(1),
                TaskPriority.Medium
            );
            
            _mockProjectTaskRepository.GetAsync(taskId).Returns(task);

            // Act
            var result = await _projectTaskManager.AddCommentAsync(taskId, content, userId);

            // Assert
            result.ShouldNotBeNull();
            result.Content.ShouldBe(content);
            result.TaskId.ShouldBe(taskId);

            // Verificar se o comentário foi salvo
            await _mockTaskCommentRepository.Received(1).InsertAsync(
                Arg.Is<TaskComment>(c => 
                    c.TaskId == taskId && 
                    c.Content == content
                )
            );

            // Verificar se o histórico foi registrado
            await _mockTaskHistoryRepository.Received(1).InsertAsync(
                Arg.Is<TaskHistory>(h => 
                    h.TaskId == taskId && 
                    h.FieldName == "Comment" && 
                    h.NewValue == content &&
                    h.ChangedByUserId == userId
                )
            );
        }
    }
}