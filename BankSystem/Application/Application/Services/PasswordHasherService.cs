using System.Security.Cryptography;
using System.Text;

namespace BankSystem.Application.Application.Services;

public class PasswordHasherService
{
    public static string HashPassword(string password)
    {
        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
    }

    public static bool VerifyPassword(string password, string hash)
    {
        return HashPassword(password) == hash;
    }
}