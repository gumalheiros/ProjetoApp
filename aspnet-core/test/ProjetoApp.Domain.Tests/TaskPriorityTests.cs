using System;
using Shouldly;
using ProjetoApp.Domain;
using Xunit;

namespace ProjetoApp.Tests
{
    public class TaskPriorityTests
    {
        [Fact]
        public void Should_Define_Correct_Priority_Values()
        {
            // Arrange & Act & Assert
            ((int)TaskPriority.Low).ShouldBe(0);
            ((int)TaskPriority.Medium).ShouldBe(1);
            ((int)TaskPriority.High).ShouldBe(2);
        }

        [Fact]
        public void Should_Parse_Priority_From_String()
        {
            // Arrange & Act & Assert
            Enum.Parse<TaskPriority>("Low").ShouldBe(TaskPriority.Low);
            Enum.Parse<TaskPriority>("Medium").ShouldBe(TaskPriority.Medium);
            Enum.Parse<TaskPriority>("High").ShouldBe(TaskPriority.High);
        }

        [Fact]
        public void Should_Convert_Priority_To_String()
        {
            // Arrange & Act & Assert
            TaskPriority.Low.ToString().ShouldBe("Low");
            TaskPriority.Medium.ToString().ShouldBe("Medium");
            TaskPriority.High.ToString().ShouldBe("High");
        }
    }
}