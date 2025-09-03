// Core/HappyDay.Application/Common/Security/IPasswordHasher.cs
namespace HappyDay.Application.Common.Security;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}