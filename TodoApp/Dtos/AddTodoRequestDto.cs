namespace TodoApp.Dtos
{
    public class AddTodoRequestDto
    {
        public string? Name { get; set; }
        public string? UserId { get; set; }
        public bool IsComplete { get; set; }
    }
}
