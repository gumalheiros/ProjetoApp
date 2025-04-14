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
    public class TaskCommentTests
    {
        private readonly IRepository<ProjectTask, Guid> _mockProjectTaskRepository;
        private readonly IRepository<Project, Guid> _mockProjectRepository;
        private readonly IRepository<TaskHistory, Guid> _mockTaskHistoryRepository;
        private readonly IRepository<TaskComment, Guid> _mockTaskCommentRepository;
        private readonly IGuidGenerator _mockGuidGenerator;
        private readonly IClock _mockClock;
        private readonly ProjectTaskManager _projectTaskManager;

        public TaskCommentTests()
        {
            // Configurar mocks para os repositórios
            _mockProjectTaskRepository = Substitute.For<IRepository<ProjectTask, Guid>>();
            _mockProjectRepository = Substitute.For<IRepository<Project, Guid>>();
            _mockTaskHistoryRepository = Substitute.For<IRepository<TaskHistory, Guid>>();
            _mockTaskCommentRepository = Substitute.For<IRepository<TaskComment, Guid>>();
            
            // Instanciar o ProjectTaskManager com os mocks
            _projectTaskManager = new ProjectTaskManager(
                _mockProjectTaskRepository,
                _mockProjectRepository,
                _mockTaskHistoryRepository,
                _mockTaskCommentRepository
            );
        }

        [Fact]
        public async Task Should_Add_Comment_To_Task()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var commentContent = "Este é um comentário de teste";

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
            var result = await _projectTaskManager.AddCommentAsync(taskId, commentContent, userId);

            // Assert
            result.ShouldNotBeNull();
            result.TaskId.ShouldBe(taskId);
            result.Content.ShouldBe(commentContent);

            // Verificar se o comentário foi salvo corretamente
            await _mockTaskCommentRepository.Received(1).InsertAsync(
                Arg.Is<TaskComment>(c =>
                    c.TaskId == taskId &&
                    c.Content == commentContent
                )
            );
        }

        [Fact]
        public async Task Should_Register_Comment_In_Task_History()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var commentContent = "Este é um comentário que deve ser registrado no histórico";

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
            // Verificar se uma entrada no histórico foi criada
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

        [Fact]
        public async Task Should_Return_Stored_Comment()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var commentContent = "Verificando o retorno do comentário";

            var task = new ProjectTask(
                taskId,
                Guid.NewGuid(),
                "Test Task",
                "Description",
                DateTime.Now.AddDays(1),
                TaskPriority.Medium
            );

            _mockProjectTaskRepository.GetAsync(taskId).Returns(task);

            // Configurando o mock para retornar o objeto inserido
            _mockTaskCommentRepository
                .When(x => x.InsertAsync(Arg.Any<TaskComment>()))
                .Do(x =>
                {
                    var comment = x.Arg<TaskComment>();
                    // Aqui podemos configurar mais propriedades se necessário
                });

            // Act
            var result = await _projectTaskManager.AddCommentAsync(taskId, commentContent, userId);

            // Assert
            result.ShouldNotBeNull();
            result.Content.ShouldBe(commentContent);
        }
    }
}