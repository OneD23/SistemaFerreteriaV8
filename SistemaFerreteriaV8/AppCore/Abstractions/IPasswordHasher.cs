namespace SistemaFerreteriaV8.AppCore.Abstractions;

public interface IPasswordHasher
{
    string Hash(string plainTextPassword);
    bool Verify(string plainTextPassword, string hash);
    bool IsHash(string candidate);
}
