using CleanArcOgrNotSis.Domain.Entities;

namespace CleanArcOgrNotSis.Domain.Interfaces;

public interface IAdminRepository
{
    Task<Admin?> IdIleGetirAsync(int id, CancellationToken cancellationToken);
    Task<Admin?> EmailIleGetirAsync(string email, CancellationToken cancellationToken);
    Task<Admin> EkleAsync(Admin admin);
}