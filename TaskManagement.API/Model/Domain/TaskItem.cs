namespace TaskManagement.API.Model.Domain
{
    public class TaskItem
    {
        public Guid PublicId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TskStatus Status { get; set; }
        public TaskPriority Prioroty { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public Guid AssignedToId { get; set; }

        //Navigation property
        public User AssignedTo { get; set; }
    }
}
