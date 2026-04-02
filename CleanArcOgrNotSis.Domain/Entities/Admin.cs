namespace CleanArcOgrNotSis.Domain.Entities;

public class Admin
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Soyad { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SifreHash { get; set; } = string.Empty;
}