using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.Security.Cryptography;

namespace TodoApp.Models
{
    public class User
    {
        public User(string? username, string? password)
        {
            UserName = username;
            Password = password;
            
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? UserName{ get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; } = false;

       
    }
}
