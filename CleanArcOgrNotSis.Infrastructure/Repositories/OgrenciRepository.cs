using CleanArcOgrNotSis.Domain.Entities;
using CleanArcOgrNotSis.Domain.Interfaces;
using CleanArcOgrNotSis.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArcOgrNotSis.Infrastructure.Repositories;

public class OgrenciRepository : IOgrenciRepository
{
    private readonly OgrenciNotSistemiDbContext _context;

    public OgrenciRepository(OgrenciNotSistemiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Ogrenci>> TumOgrencileriGetir(CancellationToken cancellationToken)
    {
        return await _context.Ogrenciler.ToListAsync(cancellationToken);
    }

    public async Task<Ogrenci?> IdIleGetir(int id, CancellationToken cancellationToken)
    {
        return await _context.Ogrenciler.FindAsync(id, cancellationToken);
    }

    public async Task<Ogrenci?> EmailIleGetirAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Ogrenciler.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<Ogrenci> EkleAsync(Ogrenci ogrenci)
    {
        _context.Ogrenciler.Add(ogrenci);
        await _context.SaveChangesAsync();
        return ogrenci;
    }

    public async Task GuncelleAsync(Ogrenci ogrenci)
    {
        _context.Ogrenciler.Update(ogrenci);
        await _context.SaveChangesAsync();
    }

    public async Task SilAsync(int id)
    {
        var ogrenci = await _context.Ogrenciler.FindAsync(id);
        if (ogrenci != null)
        {
            _context.Ogrenciler.Remove(ogrenci);
            await _context.SaveChangesAsync();
        }
    }
}