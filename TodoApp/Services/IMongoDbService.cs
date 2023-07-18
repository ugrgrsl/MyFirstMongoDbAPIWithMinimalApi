namespace TodoApp.Services
{
    public interface IMongoDbService
    {
        public Task<Todo> GetTodoAsync(string id);
        public Task<List<Todo>> GetAllTodos();
        public Task<Todo> AddTodo(Todo todo); 
    }
}
