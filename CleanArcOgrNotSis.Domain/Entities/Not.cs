namespace CleanArcOgrNotSis.Domain.Entities;

public class Not
{
    public int Id { get; set; }
    public int OgrenciId { get; set; }
    public int DersId { get; set; }
    public double Deger { get; set; }
    public DateTime Tarih { get; set; }

    public string OgrenciAdSoyad { get; set; } = string.Empty;
    public string DersAd { get; set; } = string.Empty;
    public string DersKod { get; set; } = string.Empty;
    
    public Ogrenci? Ogrenci { get; set; }
    public Ders? Ders { get; set; }
}