namespace TaskManagement.API.Model.Domain
{
    public class Enum
    {
        public enum TskStatus
        {
            Open = 1,
            InProgress = 2,
            OnHold = 3,
            Completed = 4,
        }
        public enum TaskPriority
        {
            Urgent = 1,
            High = 2,
            Medium = 3,
            Low = 4,
        }
    }
}
