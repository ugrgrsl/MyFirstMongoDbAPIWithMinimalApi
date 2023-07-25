using TodoApp.Dtos;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.EndpointHandlers
{
    public class UserEndpointHandler
    {
        private readonly IMongoDbUserService _mongoDbUser;
        public UserEndpointHandler(MongoDbUserService mongoDbUser)
        {
            _mongoDbUser = mongoDbUser;
        }
        public async Task<List<User>> GetAllUser()
        {
            return await _mongoDbUser.GetAllUsers();
        }
        public async Task<IResult> GetUserById(string id)
        {
            var data = await _mongoDbUser.GetUserById(id);
            if (data == null) return Results.NotFound();
            return Results.Ok(data);
        }
        public async Task<IResult> Login(LoginReqDto dto)
        {
            var user = await _mongoDbUser.LoginUser(dto);
            if (user == null) return Results.NotFound();
            return Results.Ok(user);
        }
        public async Task<IResult> Register(RegisterDto newUser)
        {
            var data = await _mongoDbUser.RegisterUser(newUser);
            return Results.Ok(data);
        }
        public async Task<IResult> UpdateUser(User newUser)
        {
            var data = await _mongoDbUser.UpdateUser(newUser);
            if (data == null) Results.NotFound("Tere is no user like that");
            return Results.Ok(data);
        }
        public async Task<IResult> DeleteUserById(string id)
        {
            var data = await _mongoDbUser.DeleteUserById(id);
            if (data == null) return Results.NotFound("this user is not exist");
            return Results.Ok(data);
        }
    }
}
