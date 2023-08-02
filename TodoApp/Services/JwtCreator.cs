using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class JwtCreator
    {
        private const string TokenSecret = "MySuperSecretKeyWithAtLeast128Bits";
        //TODO: read key from env file
        private static readonly TimeSpan TokenLifeTime = TimeSpan.FromMinutes(15);
         
        public static string CreateJwt(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(TokenSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("IsAdmin", user.IsAdmin.ToString()),
                    
                    // Eğer kullanıcınızın rolleri varsa, burada rolleri de ekleyebilirsiniz.
                }),
                Issuer= "https://localhost:7172/",
                Audience= "https://localhost:7172/",
                Expires = DateTime.UtcNow.Add(TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
