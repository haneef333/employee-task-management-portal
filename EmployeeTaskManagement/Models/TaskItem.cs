namespace EmployeeTaskManagement.Models
{
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }

    public class TaskItem : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; } = TaskStatus.ToDo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime DueDate { get; set; }

        // Who the task is assigned to
        public string AssignedToUserId { get; set; } = string.Empty;
        public ApplicationUser? AssignedTo { get; set; }

        // Who created the task (the manager)
        public string CreatedByUserId { get; set; } = string.Empty;
        public ApplicationUser? CreatedBy { get; set; }
    }
}