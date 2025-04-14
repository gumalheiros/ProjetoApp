using System;
using System.Threading.Tasks;
using NSubstitute;
using ProjetoApp.Domain;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Timing;
using Xunit;

namespace ProjetoApp.Tests
{
    public class TaskHistoryTests
    {
        private readonly IRepository<ProjectTask, Guid> _mockProjectTaskRepository;
        private readonly IRepository<Project, Guid> _mockProjectRepository;
        private readonly IRepository<TaskHistory, Guid> _mockTaskHistoryRepository;
        private readonly IRepository<TaskComment, Guid> _mockTaskCommentRepository;
        private readonly IGuidGenerator _mockGuidGenerator;
        private readonly IClock _mockClock;
        private readonly ProjectTaskManager _projectTaskManager;

        public TaskHistoryTests()
        {
            // Configurar mocks para os repositórios
            _mockProjectTaskRepository = Substitute.For<IRepository<ProjectTask, Guid>>();
            _mockProjectRepository = Substitute.For<IRepository<Project, Guid>>();
            _mockTaskHistoryRepository = Substitute.For<IRepository<TaskHistory, Guid>>();
            _mockTaskCommentRepository = Substitute.For<IRepository<TaskComment, Guid>>();
            
            // Instanciar o TestableProjectTaskManager com os mocks
            _projectTaskManager = new ProjectTaskManager(
                _mockProjectTaskRepository,
                _mockProjectRepository,
                _mockTaskHistoryRepository,
                _mockTaskCommentRepository
            );
        }

        [Fact]
        public async Task Should_Record_History_When_Task_Status_Changes()
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
                    h.ChangedByUserId == userId &&
                    h.ChangeType == "Update"
                )
            );
        }

        [Fact]
        public async Task Should_Record_History_When_Task_Is_Completed()
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
            await _projectTaskManager.UpdateStatusAsync(taskId, ProjectTaskStatus.Completed, userId);

            // Assert
            await _mockTaskHistoryRepository.Received(1).InsertAsync(
                Arg.Is<TaskHistory>(h => 
                    h.TaskId == taskId && 
                    h.FieldName == "Status" && 
                    h.OldValue == ProjectTaskStatus.Pending.ToString() && 
                    h.NewValue == ProjectTaskStatus.Completed.ToString() &&
                    h.ChangedByUserId == userId
                )
            );
        }

        [Fact]
        public async Task Should_Record_History_When_Comment_Added()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var commentContent = "Esse é um comentário de teste";
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
            await _projectTaskManager.AddCommentAsync(taskId, commentContent, userId);

            // Assert
            await _mockTaskHistoryRepository.Received(1).InsertAsync(
                Arg.Is<TaskHistory>(h => 
                    h.TaskId == taskId && 
                    h.FieldName == "Comment" && 
                    h.NewValue == commentContent &&
                    h.ChangedByUserId == userId &&
                    h.ChangeType == "Comment"
                )
            );
        }
    }
}