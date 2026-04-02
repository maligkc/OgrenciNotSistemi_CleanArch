using CleanArcOgrNotSis.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArcOgrNotSis.Infrastructure.Context;

public class OgrenciNotSistemiDbContext : DbContext
{
    public OgrenciNotSistemiDbContext(DbContextOptions<OgrenciNotSistemiDbContext> options) : base(options)
    {
    }

    public DbSet<Ders> Dersler { get; set; } = null!;
    public DbSet<Ogrenci> Ogrenciler { get; set; } = null!;
    public DbSet<Not> Notlar { get; set; } = null!;
    public DbSet<Ogretmen> Ogretmenler { get; set; } = null!;
    public DbSet<Admin> Adminler { get; set; } = null!;
    public DbSet<Kullanici> Kullanicilar { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ogrenci>(entity =>
        {
            entity.HasKey(e => e.Id); // Id alanını Primary Key (birincil anahtar) olarak belirler.
            
            entity.Property(e => e.Ad).IsRequired().HasMaxLength(100); 
            //IsRequired(): bu alan NULL olamaz, zorunludur
            //HasMaxLength(100): veritabanında VARCHAR(100) olarak oluşturulur
            
            entity.Property(e => e.Soyad).IsRequired().HasMaxLength(100);
            
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            
            entity.Property(e => e.SifreHash).IsRequired(false).HasMaxLength(500);
            
            entity.Property(e => e.Numara).IsRequired().HasMaxLength(20);
            
            entity.HasIndex(e => e.Numara).IsUnique();
            // HasIndex: Numara alanına index ekler (sorgular hızlanır)
            // IsUnique(): aynı numaradan iki öğrenci olamaz
        });

        modelBuilder.Entity<Ders>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Ad).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Kod).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.Kod).IsUnique();
        });

        modelBuilder.Entity<Not>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Deger).IsRequired();
            // Not değeri zorunlu, null olamaz

            entity.HasOne(e => e.Ogrenci)         // Notun bir Ogrencisi var
                .WithMany(e => e.Notlar)      // Ogrencinin birçok Notu olabilir
                .HasForeignKey(e => e.OgrenciId)  // Bağlantı OgrenciId üzerinden
                .OnDelete(DeleteBehavior.Cascade);    // Ogrenci silinirse notları da silinir

            entity.HasOne(e => e.Ders)            // Notun bir öğrencisi var
                .WithMany(e => e.Notlar)         // Ogrencinin birden fazla Notu olabilir
                .HasForeignKey(e => e.DersId)     // Bağlantı DersId üzerinden
                .OnDelete(DeleteBehavior.Cascade);    // Ders silinirse notları da silinir
        });


        modelBuilder.Entity<Ogretmen>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Ad).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Soyad).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Brans).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SifreHash).IsRequired(false).HasMaxLength(500);
            
            entity.HasIndex(e => e.Email).IsUnique();
        });
        
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Ad).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Soyad).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SifreHash).IsRequired(false).HasMaxLength(500);
            
            entity.HasIndex(e => e.Email).IsUnique();
        });
        
        modelBuilder.Entity<Kullanici>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SifreHash).IsRequired();
            entity.Property(e => e.Rol).IsRequired().HasMaxLength(50);
            entity.Property(e => e.EntityId).IsRequired();
            
            entity.HasIndex(e => e.Email).IsUnique();
        });
    }
}