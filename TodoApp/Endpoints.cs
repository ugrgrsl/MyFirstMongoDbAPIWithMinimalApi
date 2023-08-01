using Microsoft.AspNetCore.Mvc;
using TodoApp.Dtos;
using TodoApp.EndpointHandlers;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp
{
    public static class Endpoints
    {

        public static void EndpointRoutes(WebApplication app)
        {
            #region endpoints

            #region UserEndpoints
            var user = app.MapGroup("/user");
            user.MapGet("/getAllUsers", async (MongoDbUserService db) =>
            {
                return await new UserEndpointHandler(db).GetAllUser();
            }).RequireAuthorization("OnlyAdmin");
            user.MapGet("/getUserById", async (string id, MongoDbUserService db) =>
            {
                return await new UserEndpointHandler(db).GetUserById(id);
            }).RequireAuthorization("OnlyAdmin");
            user.MapPost("/refreshToken", async (RequestRefreshUserDto dto, MongoDbUserService db) =>
            {
                return await new UserEndpointHandler(db).RefreshToken(dto);
            });
            user.MapPost("/login", async (LoginReqDto dto, MongoDbUserService db) =>
            {
                return await new UserEndpointHandler(db).Login(dto);
            }).AllowAnonymous();
            user.MapPost("register", async (RegisterDto newUser, MongoDbUserService db) =>
            {
                return await new UserEndpointHandler(db).Register(newUser);
            }).AllowAnonymous();
            user.MapPut("upddateUser", async (User newUser, MongoDbUserService db) =>
            {
                return await new UserEndpointHandler(db).UpdateUser(newUser);
            });
            user.MapDelete("/deleteUser", async (string id, MongoDbUserService db) =>
            {
                return await new UserEndpointHandler(db).DeleteUserById(id);
            }).RequireAuthorization("OnlyAdmin");
            #endregion

            #region TodoEndpoints
            var todoapp = app.MapGroup("/todo");
            todoapp.MapPost("/AddTodo", async ([FromBody] AddTodoRequestDto todo, MongoDbService db) =>
            {
                return await new TodoEndpointHandlers(db).AddTodo(todo);
            });
            todoapp.MapGet("/GetTodoWithId", async (string id, MongoDbService db) =>
            {
                return await new TodoEndpointHandlers(db).GetTodoWithId(id);
            });
            todoapp.MapGet("/GetAllTodos", async (MongoDbService db) =>
            {
                return await new TodoEndpointHandlers(db).GetAllTodos();
            }).RequireAuthorization("OnlyAdmin");
            todoapp.MapGet("/GetAllTodosWithUserId", async (string id, MongoDbService db) => {
                return await new TodoEndpointHandlers(db).GetTodosWithUserId(id);
            });
            todoapp.MapPut("/UpdateTodo", async (Todo todo, MongoDbService db) =>
            {
                return await new TodoEndpointHandlers(db).UpdateTodo(todo);
            });
            todoapp.MapPut("/TurnToIsCompleted", async (IsCompleteDto ısComplete, MongoDbService db) =>
            {
                return await new TodoEndpointHandlers(db).TurnToIsCompleted(ısComplete);
            });
            todoapp.MapDelete("/DeletetTodo", async ([FromBody] DeleteDTO dto, MongoDbService db) =>
            {
                return await new TodoEndpointHandlers(db).DeleteTodo(dto);
            });
            #endregion

            #endregion

        }

    }
}
