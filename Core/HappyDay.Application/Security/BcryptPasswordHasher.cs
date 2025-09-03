// Core/HappyDay.Application/Common/Security/BcryptPasswordHasher.cs
using BCryptNet = BCrypt.Net.BCrypt;

namespace HappyDay.Application.Common.Security;

public class BcryptPasswordHasher : IPasswordHasher
{
    private readonly int _workFactor;
    private readonly string? _pepper;

    public BcryptPasswordHasher(int workFactor = 12, string? pepper = null)
    {
        _workFactor = workFactor;
        _pepper = pepper;
    }

    public string Hash(string password)
    {
        var pwd = _pepper is null ? password : password + _pepper;
        return BCryptNet.HashPassword(pwd, workFactor: _workFactor);
    }

    public bool Verify(string password, string hash)
    {
        var pwd = _pepper is null ? password : password + _pepper;
        return BCryptNet.Verify(pwd, hash);
    }
}