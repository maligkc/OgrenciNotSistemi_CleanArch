using CleanArcOgrNotSis.Application.Interfaces;

namespace CleanArcOgrNotSis.Infrastructure.Services;

public class SifreService : ISifreService
{
    public string SifreHashle(string sifre)
    {
        return BCrypt.Net.BCrypt.HashPassword(sifre);
    }

    public bool SifreDogrula(string sifre, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(sifre, hash);
    }
}