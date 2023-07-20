using System.Security.Cryptography;

namespace TodoApp.Services
{
    public class PasswordHaasher
    {
        private const int SaltSize = 128 / 8;
        private const int KeySize = 256 / 8;
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA256;
        private const char Delimeter = ';';
        public static string Hash(string password)
        {
            var salt=RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, hashAlgorithmName, KeySize);
            return string.Join(Delimeter,Convert.ToBase64String(salt),Convert.ToBase64String(hash));
        }
        public static bool Verify(string passwordHash,string inputPassword)
        {
            var elements = passwordHash.Split(Delimeter);
            var salt= Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);

            var hashInput= Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, hashAlgorithmName, KeySize);
            return CryptographicOperations.FixedTimeEquals(hash, hashInput);
        }
    }
}
