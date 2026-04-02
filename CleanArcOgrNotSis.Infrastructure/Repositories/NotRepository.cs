using CleanArcOgrNotSis.Domain.Entities;
using CleanArcOgrNotSis.Domain.Interfaces;
using CleanArcOgrNotSis.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArcOgrNotSis.Infrastructure.Repositories;

public class NotRepository : INotRepository
{
    private readonly OgrenciNotSistemiDbContext _context;

    public NotRepository(OgrenciNotSistemiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Not>> TumNotlariGetirAsync(CancellationToken cancellationToken)
    {
        return await _context.Notlar
            .Include(n => n.Ogrenci)
            .Include(n => n.Ders)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Not>> OgrenciNotlariniGetir(int ogrId, CancellationToken cancellationToken)
    {
        return await _context.Notlar
            .Where(n => n.OgrenciId == ogrId)
            .Include(n => n.Ogrenci)
            .Include(n => n.Ders)
            .ToListAsync(cancellationToken);

    }

    public async Task<IEnumerable<Not>> DersNotlariniGetir(int dersId, CancellationToken cancellationToken)
    {
        return await _context.Notlar
            .Where(n => n.DersId == dersId)
            .Include(n => n.Ogrenci)
            .Include(n => n.Ders)
            .ToListAsync(cancellationToken);
    }

    public async Task<Not?> IdIleGetir(int id, CancellationToken cancellationToken)
    {
        return await _context.Notlar
            .Include(n => n.Ders)
            .Include(n => n.Ogrenci)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<Not> EkleAsync(Not not)
    {
        _context.Notlar.Add(not);
        
        await _context.Entry(not).Reference(n => n.Ogrenci).LoadAsync();
        await _context.Entry(not).Reference(n => n.Ders).LoadAsync();
        
        await _context.SaveChangesAsync();
        return not;
    }

    public async Task GuncelleAsync(Not not)
    {
        _context.Notlar.Update(not);
        await _context.SaveChangesAsync();
    }

    public async Task SilAsync(int id)
    {
        var not = await _context.Notlar.FindAsync(id);
        if (not != null)
        {
            _context.Notlar.Remove(not);
            await _context.SaveChangesAsync();
        }
    }
}