namespace CleanArcOgrNotSis.Domain.Entities;

public class Ogrenci
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Soyad { get; set; } = string.Empty;
    public string Numara { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SifreHash { get; set; } = string.Empty;
    public DateTime KayitTarihi { get; set; }
    
    public ICollection<Not> Notlar { get; set; } = new List<Not>();
}