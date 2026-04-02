namespace CleanArcOgrNotSis.Domain.Entities;

public class Ders
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Kod { get; set; } = string.Empty;
    public int Kredi { get; set; }
    
    public ICollection<Not> Notlar { get; set; } = new List<Not>();
}