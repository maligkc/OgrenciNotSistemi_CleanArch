using CleanArcOgrNotSis.Domain.Entities;
using CleanArcOgrNotSis.Domain.Interfaces;
using CleanArcOgrNotSis.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArcOgrNotSis.Infrastructure.Repositories;

public class KullaniciRepository : IKullaniciRepository
{
    private readonly OgrenciNotSistemiDbContext _context;

    public KullaniciRepository(OgrenciNotSistemiDbContext context)
    {
        _context = context;
    }

    public async Task<Kullanici?> EmailIleGetirAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Kullanicilar.FirstOrDefaultAsync(e => e.Email == email, cancellationToken);
    }

    public async Task<bool> EmailVarMi(string email, CancellationToken cancellationToken)
    {
        return await _context.Kullanicilar.AnyAsync(e => e.Email == email, cancellationToken);
    }

    public async Task<Kullanici> EkleAsync(Kullanici kullanici)
    {
        _context.Kullanicilar.Add(kullanici);
        await _context.SaveChangesAsync();
        return kullanici;
    }
}