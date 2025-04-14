using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NSubstitute;
using ProjetoApp.Domain;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace ProjetoApp.Tests
{
    public class ProjectTaskReportManagerTests : ProjetoAppDomainTestBase
    {
        private readonly IRepository<ProjectTask, Guid> _mockTaskRepository;
        private readonly IRepository<TaskHistory, Guid> _mockHistoryRepository;
        private readonly ProjectTaskReportManager _reportManager;

        public ProjectTaskReportManagerTests()
        {
            // Configurar mocks
            _mockTaskRepository = Substitute.For<IRepository<ProjectTask, Guid>>();
            _mockHistoryRepository = Substitute.For<IRepository<TaskHistory, Guid>>();

            // Instanciar o gerenciador de relatórios
            _reportManager = new ProjectTaskReportManager(
                _mockTaskRepository,
                _mockHistoryRepository
            );
        }

        [Fact]
        public async Task Should_Generate_User_Performance_Report()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.AddDays(-30);
            var endDate = DateTime.UtcNow;

            // Criar tarefas de exemplo
            var tasks = new List<ProjectTask>
            {
                CreateProjectTask(userId, TaskPriority.High, ProjectTaskStatus.Completed, startDate.AddDays(1)),
                CreateProjectTask(userId, TaskPriority.Medium, ProjectTaskStatus.Completed, startDate.AddDays(2)),
                CreateProjectTask(userId, TaskPriority.Low, ProjectTaskStatus.InProgress, startDate.AddDays(3)),
                CreateProjectTask(userId, TaskPriority.High, ProjectTaskStatus.Pending, startDate.AddDays(5))
            };

            // Criar histórico de conclusão para as tarefas completadas
            var histories = new List<TaskHistory>
            {
                CreateTaskHistory(tasks[0].Id, startDate.AddDays(6), userId), // 5 dias para concluir
                CreateTaskHistory(tasks[1].Id, startDate.AddDays(5), userId)  // 3 dias para concluir
            };

            // Configurar comportamento dos repositórios
            var tasksQueryable = tasks.AsQueryable();
            _mockTaskRepository.WithDetailsAsync().Returns(tasksQueryable);

            var historiesQueryable = histories.AsQueryable();
            _mockHistoryRepository.WithDetailsAsync().Returns(historiesQueryable);

            // Act
            var result = await _reportManager.GenerateUserPerformanceReportAsync(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(1);

            var userReport = result.First();
            userReport.UserId.ShouldBe(userId);
            userReport.TotalTasks.ShouldBe(4);
            userReport.CompletedTasks.ShouldBe(2);
            userReport.CompletionRate.ShouldBe(0.5);  // 2 de 4 tarefas completadas
            userReport.AverageCompletionTimeInDays.ShouldBeGreaterThan(0); // Deve ter um tempo médio positivo
            userReport.TasksByPriority.ShouldBe(2);   // 2 tarefas de alta prioridade
        }

        [Fact]
        public async Task Should_Return_Empty_Report_When_No_Tasks()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-30);
            var endDate = DateTime.UtcNow;

            // Configurar repositórios vazios
            _mockTaskRepository.WithDetailsAsync().Returns(new List<ProjectTask>().AsQueryable());
            _mockHistoryRepository.WithDetailsAsync().Returns(new List<TaskHistory>().AsQueryable());

            // Act
            var result = await _reportManager.GenerateUserPerformanceReportAsync(startDate, endDate);

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(0);
        }

        private ProjectTask CreateProjectTask(Guid userId, TaskPriority priority, ProjectTaskStatus status, DateTime creationTime)
        {
            var task = new ProjectTask(
                Guid.NewGuid(),
                Guid.NewGuid(), // projectId
                "Test Task",
                "Description",
                DateTime.UtcNow.AddDays(7),
                priority
            );

            // Usar reflection para definir propriedades que não são diretamente acessíveis
            var taskType = typeof(ProjectTask);
            
            // Definir CreatorId
            var creatorIdProperty = taskType.GetProperty("CreatorId");
            creatorIdProperty?.SetValue(task, userId);
            
            // Definir CreationTime
            var creationTimeProperty = taskType.GetProperty("CreationTime");
            creationTimeProperty?.SetValue(task, creationTime);
            
            // Definir status manualmente se necessário
            if (status != ProjectTaskStatus.Pending)
            {
                taskType.GetMethod("UpdateStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.Invoke(task, new object[] { status });
            }

            return task;
        }

        private TaskHistory CreateTaskHistory(Guid taskId, DateTime changedAt, Guid userId)
        {
            var history = new TaskHistory(
                Guid.NewGuid(),
                taskId,
                "Status",
                ProjectTaskStatus.InProgress.ToString(),
                ProjectTaskStatus.Completed.ToString(),
                userId,
                "Update"
            );

            // Usar reflection para definir ChangedAt que é private set
            typeof(TaskHistory)
                .GetProperty("ChangedAt")
                ?.SetValue(history, changedAt);

            return history;
        }
    }
}