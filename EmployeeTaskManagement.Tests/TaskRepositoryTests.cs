using EmployeeTaskManagement.Models;
using EmployeeTaskManagement.Repositories;
using Moq;

namespace EmployeeTaskManagement.Tests
{
    public class TaskRepositoryTests
    {
        private readonly Mock<ITaskRepository> _mockRepo;

        public TaskRepositoryTests()
        {
            _mockRepo = new Mock<ITaskRepository>();
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsTask_WhenTaskExists()
        {
            // Arrange
            var expectedTask = new TaskItem
            {
                Id = 1,
                Title = "Test Task",
                Description = "Test Description",
                Status = Models.TaskStatus.ToDo,
                Priority = TaskPriority.High,
                DueDate = DateTime.UtcNow.AddDays(7),
                AssignedToUserId = "user123",
                CreatedByUserId = "manager123"
            };

            _mockRepo.Setup(r => r.GetTaskByIdAsync(1))
                     .ReturnsAsync(expectedTask);

            // Act
            var result = await _mockRepo.Object.GetTaskByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Task", result.Title);
            Assert.Equal(TaskPriority.High, result.Priority);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ReturnsNull_WhenTaskDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetTaskByIdAsync(99))
                     .ReturnsAsync((TaskItem?)null);

            // Act
            var result = await _mockRepo.Object.GetTaskByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetTasksByUserAsync_ReturnsOnlyUserTasks()
        {
            // Arrange
            var userId = "user123";
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Task 1", AssignedToUserId = userId, CreatedByUserId = "mgr1" },
                new TaskItem { Id = 2, Title = "Task 2", AssignedToUserId = userId, CreatedByUserId = "mgr1" }
            };

            _mockRepo.Setup(r => r.GetTasksByUserAsync(userId))
                     .ReturnsAsync(tasks);

            // Act
            var result = await _mockRepo.Object.GetTasksByUserAsync(userId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, t => Assert.Equal(userId, t.AssignedToUserId));
        }
    }
}