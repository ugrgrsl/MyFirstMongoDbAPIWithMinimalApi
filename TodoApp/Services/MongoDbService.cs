
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TodoApp.Services
{
    public class MongoDbService:IMongoDbService
    {
        private readonly IMongoCollection<Todo> _collection;
        public MongoDbService()
        {
            
        }
        public MongoDbService(IOptions<MongoDbSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionURI);
            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
            _collection=mongoDatabase.GetCollection<Todo>(dbSettings.Value.CollectionName);   
        }

        public async Task<Todo> AddTodo(Todo todo) { 
            await _collection.InsertOneAsync(todo); 
        return todo;
        }
        

        public async Task<List<Todo>> GetAllTodos() =>await _collection.Find(_=>true).ToListAsync();

        public async Task<Todo> GetTodoAsync(string id)
        {
          var data= await _collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            Todo todo=new Todo()
            {
                Id=data.Id,
                Name=data.Name,
                IsComplete=data.IsComplete
            };
            return todo;
        }
            
        
    }
}
