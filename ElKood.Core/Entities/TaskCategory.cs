using ElKood.Core.Abstractions;

namespace ElKood.Core.Entities
{
    public class TaskCategory : IHaveSoftDelete
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ToDoTask> Tasks { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
