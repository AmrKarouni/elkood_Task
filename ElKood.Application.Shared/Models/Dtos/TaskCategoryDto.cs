namespace ElKood.Application.Shared.Models.Dtos
{
    public class TaskCategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ToDoTaskDto> Tasks { get; set; }
    }
}
