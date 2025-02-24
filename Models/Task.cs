namespace MultiTenantTaskManager.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // To Do, In Progress, Completed
        public string AssignedToUserId { get; set; }
        public string TenantId { get; set; }
    }
}
