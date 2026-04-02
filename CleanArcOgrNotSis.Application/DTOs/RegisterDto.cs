namespace CleanArcOgrNotSis.Application.DTOs;

public class RegisterDto
{
    // public string Ad { get; set; } = string.Empty;
    // public string Soyad { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Sifre { get; set; } = string.Empty;
    public string SifreTekrar { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty; // "Ogrenci", "Ogretmen", "Admin"
}