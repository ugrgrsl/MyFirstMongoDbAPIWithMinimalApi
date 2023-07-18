using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Runtime;
using System.Runtime.CompilerServices;
using TodoApp;
using TodoApp.Endpoints;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<MongoDbService>();
var app = builder.Build();
//var todoItems = app.MapGroup("/todoitems");
//todoItems.MapGet("/", CrudEndpoint.GetAllTodos);
//todoItems.MapGet("/complete", CrudEndpoint.GetCompleteTodos);
//todoItems.MapGet("/{id}", CrudEndpoint.GetTodo);
//todoItems.MapPost("/", CrudEndpoint.CreateTodo);
//todoItems.MapPut("/{id}", CrudEndpoint.UpdateTodo);
//todoItems.MapDelete("/{id}", CrudEndpoint.DeleteTodo);
//app.MapGet("/",CrudEndpoint.GetCompleteTodos) ;
//app.MapPost("/", (TodoModel todo) => CrudEndpoint.AddTodoItem(todo));
app.MapPost("/", async (Todo todo, MongoDbService db) =>
{
    var newTodo = new Todo()
    {
        Id = todo.Id,
        Name = todo.Name,
        IsComplete = false
    };
    return await db.AddTodo(newTodo);
    
});
    app.MapGet("/GetTodoWithId", async (string id, MongoDbService db) =>
    {
       
        var data= await db.GetTodoAsync(id);
        return data;
    });
app.MapGet("/GetAllTodos", async (MongoDbService db) =>
{
    return await db.GetAllTodos();
}
);
app.Run();

