using CleanArcOgrNotSis.Domain.Entities;

namespace CleanArcOgrNotSis.Domain.Interfaces;

public interface IOgrenciRepository
{
    Task<IEnumerable<Ogrenci>> TumOgrencileriGetir(CancellationToken cancellationToken);
    Task<Ogrenci?> IdIleGetir(int id, CancellationToken cancellationToken);
    Task<Ogrenci?> EmailIleGetirAsync(string email, CancellationToken cancellationToken);
    Task<Ogrenci> EkleAsync(Ogrenci ogrenci);
    
    Task GuncelleAsync(Ogrenci ogrenci);
    Task SilAsync(int id);

}