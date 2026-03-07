using System.Security.Cryptography;
using System.Text;

namespace ArchitectureKata.TodoList.Cqrs;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = SHA512.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    public static bool Verify(string password, string passwordHash)
    {
        return Hash(password) == passwordHash;
    }
}
