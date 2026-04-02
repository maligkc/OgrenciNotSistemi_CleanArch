namespace CleanArcOgrNotSis.Application.DTOs;

public class AuthResponseDto
{
    public bool Basarili { get; set; }
    public string Mesaj { get; set; } = string.Empty;
    public string? Token { get; set; }
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public string? Email { get; set; }
    public string? Rol { get; set; }
}