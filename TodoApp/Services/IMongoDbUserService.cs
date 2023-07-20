using TodoApp.Dtos;
using TodoApp.Models;

namespace TodoApp.Services
{
    public interface IMongoDbUserService
    {
        public Task<User> GetUserById(string id);
        public Task<List<User>> GetAllUsers();
        public Task<ResponseLoginUserDto> LoginUser(LoginReqDto user);
        public Task<User> RegisterUser(RegisterDto newUser);
        public Task<User> UpdateUser(User newUser);
        public Task<User> DeleteUserById(string id);
    }
}
