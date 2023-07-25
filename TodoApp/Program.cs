using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Runtime;
using System.Runtime.CompilerServices;
using TodoApp;
using TodoApp.Dtos;
using TodoApp.Models;
using TodoApp.Services;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApp.EndpointHandlers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbTodo"));
builder.Services.AddSingleton<MongoDbService>();

builder.Services.Configure<MongoDbUsersSettings>(builder.Configuration.GetSection("MongoDbUser"));
builder.Services.AddSingleton<MongoDbUserService>();

// Add Swagger configuration in the Program.cs file
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoApp API :)", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type=SecuritySchemeType.ApiKey
    });

});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])!),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew=TimeSpan.FromMinutes(5)
    }
    ) ;
builder.Services.AddAuthorization(options =>
{
    // tüm endpointleri required authorize eder.
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
    options.AddPolicy("OnlyAdmin",policy =>
    {
        policy.RequireClaim("IsAdmin", "True");
    } );

});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApp API v1");
    c.RoutePrefix = string.Empty; // To serve the Swagger UI at the root URL
});
};
app.UseAuthentication();
app.UseAuthorization();
#region endpoints

#region UserEndpoints
var user =app.MapGroup("/user");
user.MapGet("/getAllUsers", async (MongoDbUserService db) =>
{
    return await new UserEndpointHandler(db).GetAllUser();
}).RequireAuthorization("OnlyAdmin");
user.MapGet("/getUserById", async (string id, MongoDbUserService db) => 
{ 
   return await new UserEndpointHandler(db).GetUserById(id);
}).RequireAuthorization("OnlyAdmin");
user.MapPost("/login", async (LoginReqDto dto,MongoDbUserService db) =>
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
todoapp.MapGet("/GetAllTodosWithUserId",async(string id, MongoDbService db)=>{
   return await new TodoEndpointHandlers(db).GetTodosWithUserId(id);
});
todoapp.MapPut("/UpdateTodo", async (Todo todo, MongoDbService db) =>
{
    return await new TodoEndpointHandlers(db).UpdateTodo(todo);
});
todoapp.MapPut("/TurnToIsCompleted", async (IsCompleteDto ýsComplete, MongoDbService db) =>
{
    return await new TodoEndpointHandlers(db).TurnToIsCompleted(ýsComplete);
});
todoapp.MapDelete("/DeletetTodo", async ([FromBody]DeleteDTO dto, MongoDbService db) =>
{
return await new TodoEndpointHandlers(db).DeleteTodo(dto);
});
#endregion

#endregion
app.Run();