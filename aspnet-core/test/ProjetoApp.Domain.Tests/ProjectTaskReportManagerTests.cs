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

        [Fact]
        public async Task Should_Calculate_Tasks_By_Priority_Correctly()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.AddDays(-30);
            var endDate = DateTime.UtcNow;

            // Criar tarefas com diferentes prioridades
            var tasks = new List<ProjectTask>
            {
                CreateProjectTask(userId, TaskPriority.High, ProjectTaskStatus.Completed, startDate.AddDays(1)),
                CreateProjectTask(userId, TaskPriority.High, ProjectTaskStatus.InProgress, startDate.AddDays(2)),
                CreateProjectTask(userId, TaskPriority.Medium, ProjectTaskStatus.Completed, startDate.AddDays(3)),
                CreateProjectTask(userId, TaskPriority.Medium, ProjectTaskStatus.Pending, startDate.AddDays(4)),
                CreateProjectTask(userId, TaskPriority.Low, ProjectTaskStatus.Completed, startDate.AddDays(5)),
                CreateProjectTask(userId, TaskPriority.Low, ProjectTaskStatus.InProgress, startDate.AddDays(6))
            };

            // Histórico para as tarefas completadas
            var histories = new List<TaskHistory>
            {
                CreateTaskHistory(tasks[0].Id, startDate.AddDays(3), userId),
                CreateTaskHistory(tasks[2].Id, startDate.AddDays(5), userId),
                CreateTaskHistory(tasks[4].Id, startDate.AddDays(7), userId)
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
            userReport.TasksByPriority.ShouldBe(2); // 2 tarefas de alta prioridade
            userReport.TotalTasks.ShouldBe(6); // Total de 6 tarefas
            userReport.CompletedTasks.ShouldBe(3); // 3 tarefas completas
            userReport.CompletionRate.ShouldBe(0.5); // 3 de 6 = 50%
        }

        [Fact]
        public async Task Should_Calculate_Average_Completion_Time_Correctly()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.AddDays(-30);
            var endDate = DateTime.UtcNow;

            // Criar tarefas de exemplo com datas específicas
            var task1 = CreateProjectTask(userId, TaskPriority.High, ProjectTaskStatus.Completed, startDate.AddDays(1));
            var task2 = CreateProjectTask(userId, TaskPriority.Medium, ProjectTaskStatus.Completed, startDate.AddDays(2));
            
            var tasks = new List<ProjectTask> { task1, task2 };

            // Criar histórico com tempos de conclusão conhecidos
            // task1: criada em startDate+1, concluída em startDate+3 = 2 dias
            // task2: criada em startDate+2, concluída em startDate+6 = 4 dias
            var histories = new List<TaskHistory>
            {
                CreateTaskHistory(task1.Id, startDate.AddDays(3), userId), // 2 dias para completar
                CreateTaskHistory(task2.Id, startDate.AddDays(6), userId)  // 4 dias para completar
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
            var userReport = result.First();
            
            // O tempo médio deve ser (2 + 4) / 2 = 3 dias
            userReport.AverageCompletionTimeInDays.ShouldBe(3);
        }

        [Fact]
        public async Task Should_Return_Zero_Average_Completion_Time_When_No_Completed_Tasks()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var startDate = DateTime.UtcNow.AddDays(-30);
            var endDate = DateTime.UtcNow;

            // Criar tarefas que não estão concluídas
            var tasks = new List<ProjectTask>
            {
                CreateProjectTask(userId, TaskPriority.High, ProjectTaskStatus.InProgress, startDate.AddDays(1)),
                CreateProjectTask(userId, TaskPriority.Medium, ProjectTaskStatus.Pending, startDate.AddDays(2)),
            };

            // Não há histórico de conclusão
            var histories = new List<TaskHistory>();

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
            userReport.TotalTasks.ShouldBe(2);
            userReport.CompletedTasks.ShouldBe(0);
            userReport.CompletionRate.ShouldBe(0);
            userReport.AverageCompletionTimeInDays.ShouldBe(0); // Não há tarefas concluídas, média é zero
        }

        [Fact]
        public void Should_Return_Correct_Keys_From_TaskPerformanceReport()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var startDate = new DateTime(2025, 3, 1);
            var endDate = new DateTime(2025, 3, 31);
            
            var report = new TaskPerformanceReport(
                userId,
                10,  // totalTasks
                5,   // completedTasks
                0.5, // completionRate
                3.5, // averageCompletionTimeInDays
                2,   // tasksByPriority
                startDate,
                endDate
            );

            // Act
            var keys = report.GetKeys();

            // Assert
            keys.ShouldNotBeNull();
            keys.Length.ShouldBe(3);
            keys[0].ShouldBe(userId);
            keys[1].ShouldBe(startDate);
            keys[2].ShouldBe(endDate);
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