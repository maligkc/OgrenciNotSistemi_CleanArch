using CleanArcOgrNotSis.Domain.Entities;
using CleanArcOgrNotSis.Domain.Interfaces;
using CleanArcOgrNotSis.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArcOgrNotSis.Infrastructure.Repositories;

public class DersRepositories : IDersRepository
{
    private readonly OgrenciNotSistemiDbContext _context;

    public DersRepositories(OgrenciNotSistemiDbContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<Ders>> TumDersleriGetirAsync(CancellationToken cancellationToken)
    {
        return await _context.Dersler.ToListAsync(cancellationToken);
    }

    public async Task<Ders?> IdIleGetirAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Dersler.FindAsync(id,cancellationToken);
    }

    public async Task<Ders> EkleAsync(Ders ders)
    {
        _context.Dersler.Add(ders);
        await _context.SaveChangesAsync();
        return ders;
    }

    public async Task SilAsync(int id)
    {
        var ders = await _context.Dersler.FindAsync(id);
        if (ders != null)
        {
            _context.Dersler.Remove(ders);
            await _context.SaveChangesAsync();
        }
    }

    public async Task GuncelleAsync(Ders ders)
    {
        _context.Dersler.Update(ders);
        await _context.SaveChangesAsync();
    }
}