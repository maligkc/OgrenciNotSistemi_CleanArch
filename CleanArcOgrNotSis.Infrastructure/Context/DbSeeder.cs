using System.Text.Json;
using CleanArcOgrNotSis.Application.Interfaces;
using CleanArcOgrNotSis.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArcOgrNotSis.Infrastructure.Context;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OgrenciNotSistemiDbContext>();
        var sifreService = scope.ServiceProvider.GetRequiredService<ISifreService>();

        // Zaten admin varsa tekrar ekleme
        if (context.Kullanicilar.Any())
            return;

        // seed-data.json dosyasını bul
        var path = Path.Combine(AppContext.BaseDirectory, "Data", "seed-data.json");
        if (!File.Exists(path))
        {
            path = Path.Combine(Directory.GetCurrentDirectory(), "..", "CleanArcOgrNotSis.Infrastructure", "Data", "seed-data.json");
        }

        if (!File.Exists(path)) return;

        var json = await File.ReadAllTextAsync(path);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var data = JsonSerializer.Deserialize<SeedDataDto>(json, options);

        if (data?.Admins == null) return;

        foreach (var item in data.Admins)
        {
            var hash = sifreService.SifreHashle(item.Password);

            var admin = new Admin
            {
                Ad = item.Ad,
                Soyad = item.Soyad,
                Email = item.Email,
                SifreHash = hash
            };
            await context.Adminler.AddAsync(admin);
            await context.SaveChangesAsync();

            await context.Kullanicilar.AddAsync(new Kullanici
            {
                Email = admin.Email,
                SifreHash = hash,
                Rol = "Admin",
                EntityId = admin.Id,
                KayitTarihi = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }
    }

    private class SeedDataDto
    {
        public List<AdminSeedDto>? Admins { get; set; }
    }

    private class AdminSeedDto
    {
        public string Ad { get; set; } = "";
        public string Soyad { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
