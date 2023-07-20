using System.Text.Json.Serialization;

namespace TodoApp.Dtos
{
    public class RegisterDto
    {
        public RegisterDto(string username,string password)
        {
            Username=username;
            Password=password;
        }
        public string Username { get; set; }
        public string  Password { get; set; }

    }
}
