using TodoApp.Dtos;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.EndpointHandlers
{
    public class TodoEndpointHandlers
    {
        
        private readonly IMongoDbService _mongoDbService;
       
        public TodoEndpointHandlers(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }
        public async Task<Todo> AddTodo(AddTodoRequestDto dto)
        {
            return await _mongoDbService.AddTodo(dto);
        }
        public async Task<IResult> GetTodoWithId(string id)
        {
            var data = await _mongoDbService.GetTodoAsync(id);
            if (data == null) return Results.NotFound("there is no todo with that id");
            return Results.Ok(data);
        }
        public  async Task<List<Todo>> GetAllTodos()
        {
            return await _mongoDbService.GetAllTodos();
        }
        public async Task<IResult> GetTodosWithUserId(string id)
        {
            var data = await _mongoDbService.GetTodoWithUserIdAsync(id);
            if (data == null) return Results.NotFound("This user has not any todo");
            return Results.Ok(data);
        }
        public async Task<IResult> UpdateTodo(Todo todo)
        {
            var newdata = await _mongoDbService.UpdateTodo(todo);
            if (newdata == null) return Results.BadRequest(newdata);
            return Results.Ok(newdata);
        }
     public async Task<IResult> TurnToIsCompleted(IsCompleteDto ısComplete)
        {
            var data = await _mongoDbService.TurnIscomleted(ısComplete);
            if (data == null) return Results.NotFound();
            return Results.Ok(data);
        }
        public async Task<IResult> DeleteTodo(DeleteDTO dto)
        {
            var deletedData = await _mongoDbService.DeleteTodo(dto.Id);
            if (deletedData == null) return Results.NotFound();
            return Results.Ok(deletedData);
        }
    }
}
