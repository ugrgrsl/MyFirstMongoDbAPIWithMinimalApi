using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Cryptography;
using TodoApp.Dtos;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class MongoDbUserService : IMongoDbUserService
    {
        private readonly IMongoCollection<User> _collection;
        public MongoDbUserService(IOptions<MongoDbUsersSettings> dbSettings)
        {
            var mongoClient= new MongoClient(dbSettings.Value.ConnectionURI);
            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
            _collection = mongoDatabase.GetCollection<User>(dbSettings.Value.CollectionName);
        }

        public async Task<User> DeleteUserById(string id)
        {
            var data = await _collection.Find(x=>x.Id.Equals(id)).FirstOrDefaultAsync();
            if (data == null) return null; 
            await _collection.DeleteOneAsync(id);
            return data;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var datas =await _collection.Find(_=>true).ToListAsync();
            return datas;
        }

        public async Task<User> GetUserById(string id)
        {
            var data = await _collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            if (data == null) return null;
            return data;
        }

        public async Task<ResponseLoginUserDto> LoginUser(LoginReqDto user)
        {
            var data= await _collection.Find(x=>x.UserName==user.UserName).FirstOrDefaultAsync();
            if (data == null) return null;
            var result=PasswordHaasher.Verify(data.Password,user.Password);
            if (!result) return null;
            var jwtToken = JwtCreator.CreateJwt(data);
            
            ResponseLoginUserDto responseLoginUserDto = new ResponseLoginUserDto()
            {
                Id = data.Id,
                Username = user.UserName,
                AccesToken = jwtToken
            };
            return responseLoginUserDto;
        }

        public async Task<ResponseRefreshUserDto> RefreshToken(RequestRefreshUserDto user)
        {
            var data = await _collection.Find(x => x.Id.Equals(user.UserId)).FirstOrDefaultAsync();
            if (data == null) return null;
            var token=JwtCreator.CreateJwt(data);
            ResponseRefreshUserDto response = new ResponseRefreshUserDto()
            { 
                Id= data.Id,
                UserName=data.UserName,
                RefreshToken = token
            };
            return response;
        }

        // bu alana iki dto eklemen lazım

        public async Task<User> RegisterUser(RegisterDto user)
        {
            var passwordHash = PasswordHaasher.Hash(user.Password);
            User newUser = new User(user.Username, user.Password)
            {
                IsAdmin = false,
                UserName = user.Username,
                Password = passwordHash
            };
            await _collection.InsertOneAsync(newUser);
            return newUser;
        }

        public async Task<User> UpdateUser(User newUser)
        {
            await _collection.ReplaceOneAsync(x => x.Id == newUser.Id, newUser);
            return newUser;
        }
    }
}
