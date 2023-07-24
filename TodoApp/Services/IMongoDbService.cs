using TodoApp.Dtos;
using TodoApp.Models;

namespace TodoApp.Services
{
    public interface IMongoDbService
    {
        public Task<List<Todo>> GetTodoWithUserIdAsync(string id);
        public Task<Todo> GetTodoAsync(string id);
        public Task<List<Todo>> GetAllTodos();
        public Task<Todo> AddTodo(AddTodoRequestDto todo); 
        public Task<Todo> UpdateTodo(Todo todo);
        public Task<Todo> TurnIscomleted(IsCompleteDto ısCompleteDto);
        public Task<Todo> DeleteTodo(string id);
    }
}
