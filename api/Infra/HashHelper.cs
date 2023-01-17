using System.Security.Cryptography;
using System.Text;

namespace GacorAPI.Infra
{
    public static class HashHelper
    {
        public static string HashPassword(string password)
        {
            using(var sha256 = SHA256.Create())
            {
                var b = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                for (var i = 0; i < b.Length; i++)
                {
                    builder.Append(b[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}