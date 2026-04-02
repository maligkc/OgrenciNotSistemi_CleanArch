using CleanArcOgrNotSis.Domain.Entities;

namespace CleanArcOgrNotSis.Domain.Interfaces;

public interface IKullaniciRepository
{
    Task<Kullanici?> EmailIleGetirAsync(string email, CancellationToken cancellationToken);
    Task<bool> EmailVarMi(string email, CancellationToken cancellationToken);
    Task<Kullanici> EkleAsync(Kullanici kullanici);
}