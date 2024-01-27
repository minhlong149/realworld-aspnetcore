using Core.Services;

namespace Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyHashedPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
}
