using CleanArcOgrNotSis.Domain.Entities;

namespace CleanArcOgrNotSis.Domain.Interfaces;

public interface INotRepository
{
    Task<IEnumerable<Not>> TumNotlariGetirAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Not>> OgrenciNotlariniGetir(int ogrId, CancellationToken cancellationToken);
    Task<IEnumerable<Not>> DersNotlariniGetir(int dersId, CancellationToken cancellationToken);

    Task<Not?> IdIleGetir(int id, CancellationToken cancellationToken);
    Task<Not> EkleAsync(Not not);
    Task GuncelleAsync(Not not);
    Task SilAsync(int id);

}