namespace CleanArcOgrNotSis.Application.DTOs;

public class OgrenciDto
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Soyad { get; set; } = string.Empty;
    public string Numara { get; set; } = string.Empty;
    public DateTime KayitTarhi { get; set; }
}