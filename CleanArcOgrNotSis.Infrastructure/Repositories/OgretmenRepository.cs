using CleanArcOgrNotSis.Domain.Entities;
using CleanArcOgrNotSis.Domain.Interfaces;
using CleanArcOgrNotSis.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArcOgrNotSis.Infrastructure.Repositories;

public class OgretmenRepository : IOgretmenRepository
{
    private readonly OgrenciNotSistemiDbContext _context;

    public async Task<IEnumerable<Ogretmen>> TumOgretmenleriGetirAsync(CancellationToken cancellationToken)
    {
        return await _context.Ogretmenler.ToListAsync(cancellationToken);
    }

    public async Task<Ogretmen?> IdIleGetirAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Ogretmenler.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Ogretmen?> EmailIleGetirAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Ogretmenler.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<Ogretmen> EkleAsync(Ogretmen ogretmen)
    {
        _context.Ogretmenler.Add(ogretmen);
        await _context.SaveChangesAsync();
        return ogretmen;
    }

    public async Task GuncelleAsync(Ogretmen ogretmen)
    {
        _context.Ogretmenler.Update(ogretmen);
        await _context.SaveChangesAsync();
    }

    public async Task SilAsync(int id)
    {
        var ogretmen = await _context.Ogretmenler.FindAsync(id);
        if (ogretmen != null)
        {
            _context.Ogretmenler.Remove(ogretmen);
            await _context.SaveChangesAsync();
        }
    }
}