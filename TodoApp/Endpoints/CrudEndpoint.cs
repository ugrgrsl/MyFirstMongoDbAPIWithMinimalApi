using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using TodoApp.Services;

namespace TodoApp.Endpoints
{
    public class CrudEndpoint
    {

        private readonly MongoDbService _mongodbService;

        public CrudEndpoint(MongoDbService mongodbService)
        {
            _mongodbService = mongodbService;
        }

        public async Task<Todo> AddOneToDo(TodoModel todo)
        {
            var newTodo = new Todo()
            {
                Id = "1",
                Name = todo.Content,
                IsComplete = true,
            };
            await _mongodbService.AddTodo(newTodo);
            return newTodo;
        }

        //  public async Task<List<IResult>> GetAllTodos() => await _mongodbService.GetTodosAsync();

        public static async Task<IResult> GetCompleteTodos(string id)
        {
            return TypedResults.Ok("GetCompleteTodos "+id);
        }

        // alttakie nedense istek atamıyorum

        /*public async Task<IResult> GetTodo(string id)
        {
            var data = await _mongodbService.GetAllTodos(id);
            return TypedResults.Ok(data);
        }
        */
     
    }
}
