namespace ElKood.Application.Shared.Models.Inputs
{
    public class ToDoTaskInput
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public int CategoryId { get; set; }
    }
}
