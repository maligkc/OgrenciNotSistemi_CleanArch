namespace CleanArcOgrNotSis.Domain.Entities;

public class Kullanici
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string SifreHash { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;        // "Admin", "Ogretmen", "Ogrenci"
    public int EntityId { get; set; }                       // İlgili tablodaki kayıt ID'si
    public DateTime KayitTarihi { get; set; }
}