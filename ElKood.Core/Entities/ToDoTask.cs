using ElKood.Core.Abstractions;

namespace ElKood.Core.Entities
{
    public class ToDoTask : IHaveSoftDelete
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatetionDate { get; set; }

        public int Priority { get; set; }

        public DateTime? CompletionDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; }

        public int CategoryId { get; set; }

        public virtual TaskCategory Category { get; set; }
    }
}
