using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Application.Interfaces;
using CleanArcOgrNotSis.Domain.Entities;
using CleanArcOgrNotSis.Domain.Interfaces;
using MediatR;

namespace CleanArcOgrNotSis.Application.Commands.Auth;

public class AuthCommands
{
    public record KayitOlCommand(RegisterDto RegisterDto) : IRequest<AuthResponseDto>;

    public record GirisYapCommand(LoginDto LoginDto) : IRequest<AuthResponseDto>;
}

public class KayitOlCommandHandler : IRequestHandler<AuthCommands.KayitOlCommand, AuthResponseDto>
{
    private readonly IKullaniciRepository _kullaniciRepository;
    private readonly IOgretmenRepository _ogretmenRepository;
    private readonly IOgrenciRepository _ogrenciRepository;
    private readonly IAdminRepository _adminRepository;
    private readonly IJwtService _jwtService;
    private readonly ISifreService _sifreService;

    public KayitOlCommandHandler(
        IKullaniciRepository kullaniciRepository, 
        IJwtService jwtService, 
        ISifreService sifreService, 
        IOgretmenRepository ogretmenRepository, 
        IOgrenciRepository ogrenciRepository, 
        IAdminRepository adminRepository)
    {
        _kullaniciRepository = kullaniciRepository;
        _jwtService = jwtService;
        _sifreService = sifreService;
        _ogretmenRepository = ogretmenRepository;
        _ogrenciRepository = ogrenciRepository;
        _adminRepository = adminRepository;
    }


    public async Task<AuthResponseDto> Handle(AuthCommands.KayitOlCommand request, CancellationToken cancellationToken)
    {
        var dto = request.RegisterDto;

        // 1. bu email ile daha önce hesap açılmış mı

        var hesapVarMi = await _kullaniciRepository.EmailVarMi(dto.Email, cancellationToken);
        if (hesapVarMi)
        {
            return new AuthResponseDto()
            {
                Basarili = false,
                Mesaj = "Bu email adresiyle zaten bir hesap oluşturulmuş."
            };
        }

        // 2- role göre ilgili tabloda bu email var mı kontrol et

        string ad, soyad;
        int entityId; // ilgili tablodaki verilerin id'si. Öğrenciler tablosu için ogrenci.Id, Ogretmenler için ogretmen.Id vs..
        switch (dto.Rol.ToLower())
        {
            case "ogrenci":
                var ogrenci = await _ogrenciRepository.EmailIleGetirAsync(dto.Email, cancellationToken);
                if (ogrenci == null)
                {
                    return new AuthResponseDto()
                    {
                        Basarili = false,
                        Mesaj =
                            "Sistemde bu email adresine ait öğrenci kaydı bulunmamaktadır. Lütfen idare ile iletişime geçin."
                    };
                }

                ad = ogrenci.Ad;
                soyad = ogrenci.Soyad;
                entityId = ogrenci.Id;
                break;
            case "ogretmen":
                var ogretmen = await _ogretmenRepository.EmailIleGetirAsync(dto.Email, cancellationToken);
                if (ogretmen == null)
                {
                    return new AuthResponseDto()
                    {
                        Basarili = false,
                        Mesaj =
                            "Sistemde bu email adresine ait bir öğretmen kaydı bulunmamaktadır. Lütfen idare ile iletişime geçin."
                    };
                }

                ad = ogretmen.Ad;
                soyad = ogretmen.Soyad;
                entityId = ogretmen.Id;
                break;
            case "admin":
                var admin = await _adminRepository.EmailIleGetirAsync(dto.Email, cancellationToken);
                if (admin == null)
                {
                    return new AuthResponseDto()
                    {
                        Basarili = false,
                        Mesaj = "Sistemde bu email adresine ait bir admin kaydı bulunmamaktadır."
                    };
                }
                ad = admin.Ad;
                soyad = admin.Soyad;
                entityId = admin.Id;
                break;
            default:
                return new AuthResponseDto
                {
                    Basarili = false, 
                    Mesaj = "Geçersiz rol. Lütfen 'Ogrenci', 'Ogretmen' veya 'Admin' değerlerinden birini girin."
                };

        }
        
        
        // 3- Kullanıcı tablosuna kayıt ekle

        var kullanici = new Kullanici()
        {
            Email = dto.Email,
            SifreHash = _sifreService.SifreHashle(dto.Sifre),
            Rol = dto.Rol,
            EntityId = entityId,
            KayitTarihi = DateTime.UtcNow
        };

        await _kullaniciRepository.EkleAsync(kullanici);
        
        
        // 4- token oluştur
        var token = _jwtService.TokenOlustur(entityId, dto.Email, ad, soyad, dto.Rol);

        return new AuthResponseDto()
        {
            Basarili = true,
            Mesaj = "Hesap başarıyla oluşturuldu",
            Token = token,
            Ad = ad,
            Soyad = soyad,
            Email = dto.Email,
            Rol = dto.Rol
        };

    }
}

public class GirisYapCommandHandler : IRequestHandler<AuthCommands.GirisYapCommand, AuthResponseDto>
{
    private IJwtService _jwtService;
    private ISifreService _sifreService;
    private readonly IKullaniciRepository _kullaniciRepository;
    private readonly IOgretmenRepository _ogretmenRepository;
    private readonly IOgrenciRepository _ogrenciRepository;
    private readonly IAdminRepository _adminRepository;

    public GirisYapCommandHandler( IJwtService jwtService, ISifreService sifreService, IKullaniciRepository kullaniciRepository, IOgretmenRepository ogretmenRepository, IOgrenciRepository ogrenciRepository, IAdminRepository adminRepository)
    {
        _jwtService = jwtService;
        _sifreService = sifreService;
        _kullaniciRepository = kullaniciRepository;
        _ogretmenRepository = ogretmenRepository;
        _ogrenciRepository = ogrenciRepository;
        _adminRepository = adminRepository;
    }


    public async Task<AuthResponseDto> Handle(AuthCommands.GirisYapCommand request, CancellationToken cancellationToken)
    {
        var dto = request.LoginDto;

        // kullanici tablosundan emaille sorgulama

        var kullanici = await _kullaniciRepository.EmailIleGetirAsync(dto.Email, cancellationToken);
        if (kullanici == null)
        {
            return new AuthResponseDto()
            {
                Basarili = false,
                Mesaj = "Email veya şifre hatalı, henüz hesap oluşturmadıysanız kayıt ol işlemini yapınız."
            };
        }
        
        // şifre sorgulama
        var sifreDogruMu = _sifreService.SifreDogrula(dto.Sifre, kullanici.SifreHash);

        if (!sifreDogruMu)
        {
            return new AuthResponseDto()
            {
                Basarili = false,
                Mesaj = "Email veya şifre hatalı."
            };
        }
        
        // ilgili tablodaki Ad/Soyad bilgisini çek
        string ad = "", soyad = "";

        switch (kullanici.Rol.ToLower())
        {
            case "ogrenci":
                var ogrenci = await _ogrenciRepository.IdIleGetir(kullanici.EntityId, cancellationToken);
                if (ogrenci != null)
                {
                    ad = ogrenci.Ad;
                    soyad = ogrenci.Soyad;
                }
                break;
            case "ogretmen":
                var ogretmen = await _ogretmenRepository.IdIleGetirAsync(kullanici.EntityId, cancellationToken);
                if (ogretmen != null)
                {
                    ad = ogretmen.Ad;
                    soyad = ogretmen.Soyad;
                }
                break;
            case "admin":
                var admin = await _adminRepository.IdIleGetirAsync(kullanici.EntityId, cancellationToken);
                if (admin != null)
                {
                    ad = admin.Ad;
                    soyad = admin.Soyad;
                }
                break;
        }
        
        
        // token oluştur

        var token = _jwtService.TokenOlustur(kullanici.EntityId, kullanici.Email, ad, soyad, kullanici.Rol);

        return new AuthResponseDto()
        {
            Basarili = true,
            Mesaj = "Giriş başarılı",
            Token = token,
            Ad = ad,
            Soyad = soyad,
            Email = kullanici.Email,
            Rol = kullanici.Rol
        };

    }
}