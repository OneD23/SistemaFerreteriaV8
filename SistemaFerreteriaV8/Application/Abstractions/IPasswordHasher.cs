namespace SistemaFerreteriaV8.Application.Abstractions;

public interface IPasswordHasher
{
    string Hash(string plainTextPassword);
    bool Verify(string plainTextPassword, string hash);
    bool IsHash(string candidate);
}
