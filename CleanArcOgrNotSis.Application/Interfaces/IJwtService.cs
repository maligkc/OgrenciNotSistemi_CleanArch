using CleanArcOgrNotSis.Domain.Entities;

namespace CleanArcOgrNotSis.Application.Interfaces;

public interface IJwtService
{
    string TokenOlustur(int id, string email, string ad, string soyad, string rol);
}