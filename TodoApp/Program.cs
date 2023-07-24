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
        ValidateIssuerSigningKey = true
    }
    ) ;
builder.Services.AddAuthorization(options =>
{
    // tüm endpointleri required authorize eder.
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
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
    return await db.GetAllUsers();
});
user.MapGet("/getUserById", async (string id, MongoDbUserService db) => 
{ 
    var data=await db.GetUserById(id);
    if (data == null) return Results.NotFound();
    return Results.Ok(data);
});
user.MapPost("/login", async (LoginReqDto dto,MongoDbUserService db) =>
{
    var user = await db.LoginUser(dto);
    if (user == null) return Results.NotFound();


        return Results.Ok(user);
}).AllowAnonymous();
user.MapPost("register", async (RegisterDto newUser, MongoDbUserService db) =>
{
  
    var data= await db.RegisterUser(newUser);
   
    return Results.Ok(data);
});
user.MapPut("upddateUser", async (User newUser, MongoDbUserService db) =>
{
    var data = await db.UpdateUser(newUser);
    if (data == null) Results.NotFound("Tere is no user like that");
    return Results.Ok(data);
});
user.MapDelete("/deleteUser", async (string id, MongoDbUserService db) =>
{
    var data=await db.DeleteUserById(id);
    if (data == null) return Results.NotFound("this user is not exist");
    return Results.Ok(data);
});
#endregion
#region TodoEndpoints

app.MapPost("/AddTodo", async ([FromBody] AddTodoRequestDto todo, MongoDbService db) =>
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
app.MapGet("/GetAllTodosWithUserId",async(string id, MongoDbService db)=>{
    var data=await db.GetTodoWithUserIdAsync(id);
    if (data == null) return Results.NotFound("This user has not any todo");
    return Results.Ok(data);
}).AllowAnonymous() ;
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
#endregion
#endregion
app.Run();