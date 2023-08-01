using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using TodoApp.Dtos;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class MongoDbService : IMongoDbService
    {
        private readonly IMongoCollection<Todo> _collection;
        
       
        public MongoDbService(IOptions<MongoDbSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionURI);
            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
            _collection=mongoDatabase.GetCollection<Todo>(dbSettings.Value.CollectionName);   
        }

        public async Task<Todo> AddTodo(AddTodoRequestDto todo) {
            Todo newTodo = new Todo()
            {
                Name = todo.Name,
                UserId = todo.UserId,
                IsComplete = todo.IsComplete,
            };

            await _collection.InsertOneAsync(newTodo); 
        return newTodo;
        }
     
        public async Task<Todo> DeleteTodo(string id)
        {
            var data = await _collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            if (data == null) return null;
            await _collection.DeleteOneAsync(x=>x.Id.Equals(data.Id));
            return data; ;
        }

        public async Task<List<Todo>> GetAllTodos() =>await _collection.Find(_=>true).ToListAsync();

        public async Task<Todo> GetTodoAsync(string id)
        {
          var data= await _collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            if (data == null) return null;
            Todo todo=new Todo()
            {
                Id=data.Id,
                Name=data.Name,
                IsComplete=data.IsComplete
            };
            return todo;
        }

        public async Task<List<Todo>> GetTodoWithUserIdAsync(string id)
        {
            var data = await _collection.Find(x => x.UserId.Equals(id)).ToListAsync();
            if (data == null || data.Count()==0) return null;
            return data;
        }

        public async Task<Todo> TurnIscomleted(IsCompleteDto ısCompleteDto)
        {
            var data=await _collection.Find(x=> x.Id.Equals(ısCompleteDto.Id)).FirstOrDefaultAsync();
            if (data == null) return null;
            Todo newdata = new Todo()
            {
                Id = ısCompleteDto.Id,
                Name=data.Name,
                UserId=data.UserId,
                IsComplete = ısCompleteDto.IsComplete
            };
            await _collection.ReplaceOneAsync(x=>x.Id.Equals(ısCompleteDto.Id), newdata);
            return newdata;
        }

        public async Task<Todo> UpdateTodo(Todo todo)
        {
            Todo newdata=new Todo()
            {
                Id = todo.Id,
                Name=todo.Name,
                UserId=todo.UserId,
                IsComplete=todo.IsComplete
            };
            var x= await _collection.ReplaceOneAsync(x=>x.Id.Equals(todo.Id), newdata);
            return newdata;
        }
    }
}
