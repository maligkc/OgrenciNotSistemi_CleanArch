using CleanArcOgrNotSis.Domain.Entities;

namespace CleanArcOgrNotSis.Domain.Interfaces;

public interface IOgretmenRepository
{
    Task<IEnumerable<Ogretmen>> TumOgretmenleriGetirAsync(CancellationToken cancellationToken);
    Task<Ogretmen?> IdIleGetirAsync(int id, CancellationToken cancellationToken);
    Task<Ogretmen?> EmailIleGetirAsync(string email, CancellationToken cancellationToken);
    Task<Ogretmen> EkleAsync(Ogretmen ogretmen);
    Task GuncelleAsync(Ogretmen ogretmen);
    Task SilAsync(int id);
}