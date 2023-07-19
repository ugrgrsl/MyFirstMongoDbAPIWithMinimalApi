using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Runtime;
using System.Runtime.CompilerServices;
using TodoApp;
using TodoApp.Dtos;
using TodoApp.Models;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<MongoDbService>();
var app = builder.Build();

app.MapGet("/", () => "Server is live!!!");
app.MapPost("/AddTodo", async ([FromBody]Todo todo, MongoDbService db) =>
{
    return await db.AddTodo(todo);
    
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
app.MapPut("/UpdateTodo", async (Todo todo, MongoDbService db) =>
{
    return await db.UpdateTodo(todo);
});
app.MapPut("/TurnToIsCompleted", async (IsCompleteDto ýsComplete, MongoDbService db) =>
{
    return await db.TurnIscomleted(ýsComplete);
});
app.MapDelete("/DeletetTodo", async ([FromBody]DeleteDTO dto, MongoDbService db) =>
{
 
        return await db.DeleteTodo(dto.Id);
    
});
app.Run();