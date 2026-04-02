using CleanArcOgrNotSis.Domain.Entities;

namespace CleanArcOgrNotSis.Domain.Interfaces;

public interface IDersRepository
{
    Task<IEnumerable<Ders>> TumDersleriGetirAsync(CancellationToken cancellationToken); 
    Task<Ders> IdIleGetirAsync(int id, CancellationToken cancellationToken);
    Task<Ders> EkleAsync(Ders ders);
    Task SilAsync(int id);
    Task GuncelleAsync(Ders ders);
}