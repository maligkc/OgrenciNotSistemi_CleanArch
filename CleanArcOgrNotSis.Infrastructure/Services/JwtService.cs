using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArcOgrNotSis.Application.Interfaces;
using CleanArcOgrNotSis.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace CleanArcOgrNotSis.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string TokenOlustur(int id, string email, string ad, string soyad, string rol)
    {
        var jwtAyarlari = _configuration.GetSection("JwtAyarlari");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new("id", id.ToString()), // Kolay erişim için ek bir ID claim'i
            new(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, $"{ad} {soyad}"),
            new Claim(ClaimTypes.Role, rol),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAyarlari["GizliAnahtar"]!));

        var credentals = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtAyarlari["Issuer"],
            audience: jwtAyarlari["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtAyarlari["SureDakika"]!)),
            signingCredentials: credentals
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}