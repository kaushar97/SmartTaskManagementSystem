using System.ComponentModel;

namespace TaskManagement.API.Model.Domain
{
    public class Enum
    {
        [TypeConverter(typeof(EnumConverter))]
        public enum TskStatus
        {
            Open = 1,
            InProgress = 2,
            OnHold = 3,
            Completed = 4,
        }
        [TypeConverter(typeof(EnumConverter))]
        public enum TaskPriority
        {
            Urgent = 1,
            High = 2,
            Medium = 3,
            Low = 4,
        }
    }
}
