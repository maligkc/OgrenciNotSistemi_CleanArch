namespace CleanArcOgrNotSis.Application.DTOs;

public class NotDto
{
    public int Id { get; set; }
    public int OgrenciId { get; set; } 
    public int DersId { get; set; }
    public double Deger { get; set; }
    public DateTime Tarih { get; set; }

    public string? OgrenciAdSoyad { get; set; }
    public string? DersAd { get; set; }
    public string? DersKod { get; set; }
}