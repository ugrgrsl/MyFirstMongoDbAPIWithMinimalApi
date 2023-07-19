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
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
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
        if (data == null) return Results.NotFound("there is no todo with that id");
        return Results.Ok(data);
    });
app.MapGet("/GetAllTodos", async (MongoDbService db) =>
{
    return await db.GetAllTodos();
}
);
app.MapPut("/UpdateTodo", async (Todo todo, MongoDbService db) =>
{
    var newdata=await db.UpdateTodo(todo);
    if(newdata==null) return Results.BadRequest(newdata);
    return Results.Ok(newdata);
});
app.MapPut("/TurnToIsCompleted", async (IsCompleteDto ýsComplete, MongoDbService db) =>
{
    var data=await db.TurnIscomleted(ýsComplete);
    if (data==null) return Results.NotFound();
    return Results.Ok(data);
});
app.MapDelete("/DeletetTodo", async ([FromBody]DeleteDTO dto, MongoDbService db) =>
{
var deletedData= await db.DeleteTodo(dto.Id);
    if(deletedData==null) return Results.NotFound();
    return Results.Ok(deletedData);
});
app.Run();