using CleanArcOgrNotSis.Domain.Entities;
using CleanArcOgrNotSis.Domain.Interfaces;
using CleanArcOgrNotSis.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArcOgrNotSis.Infrastructure.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly OgrenciNotSistemiDbContext _context;

    public AdminRepository(OgrenciNotSistemiDbContext context)
    {
        _context = context;
    }

    public async Task<Admin?> IdIleGetirAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Adminler.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Admin?> EmailIleGetirAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Adminler.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<Admin> EkleAsync(Admin admin)
    {
        _context.Adminler.Add(admin);
        await _context.SaveChangesAsync();
        return admin;
    }
}