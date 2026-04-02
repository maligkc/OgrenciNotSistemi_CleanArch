# Öğrenci Not Sistemi (Clean Architecture & CQRS)

Bu proje, modern yazılım mimarisi prensipleri kullanılarak geliştirilmiş kapsamlı bir **Öğrenci Not Sistemi API**'sidir. Proje, sürdürülebilirlik, test edilebilirlik ve esneklik odaklı **Clean Architecture** (Temiz Mimari) üzerine inşa edilmiştir.

## 🚀 Teknolojiler ve Yaklaşımlar

- **.NET 8 / Core**: En güncel web framework sürümü.
- **Clean Architecture**: Katmanlı mimari (Domain, Application, Infrastructure, Presentation).
- **CQRS (MediatR)**: Komut ve sorguların ayrıştırılması.
- **Entity Framework Core**: Veritabanı yönetim katmanı.
- **PostgreSQL**: İlişkisel veritabanı tercihi.
- **JWT (JSON Web Token)**: Rol tabanlı güvenli kimlik doğrulama.
- **AutoMapper**: Nesneler arası otomatik eşleme (Mapping).
- **FluentValidation**: MediatR Pipeline üzerinden akan verilerin doğrulanması.
- **Swagger / OpenAPI**: Kolay test edilebilir API dokümantasyonu.

## 🏗️ Proje Yapısı

Proje 4 ana katmandan oluşmaktadır:

1.  **CleanArcOgrNotSis.Domain**: Projenin kalbidir. Hiçbir dış katmana bağımlı değildir. 
    - **Entities**: Öğrenci, Öğretmen, Ders, Not, Kullanıcı ve Admin nesneleri.
    - **Interfaces**: Repository arayüzleri.
2.  **CleanArcOgrNotSis.Application**: İş mantığının (Use Cases) bulunduğu katman.
    - **Commands / Queries**: CQRS desenine göre yazılmış istekler.
    - **DTOs**: Veri transfer nesneleri.
    - **Validators**: FluentValidation ile veri doğrulama.
    - **Behaviors**: MediatR pipeline'ı üzerinden çalışan doğrulama logicleri.
3.  **CleanArcOgrNotSis.Infrastructure**: Dış kaynaklarla iletişimi sağlar.
    - **Context**: Entity Framework DbContext yapılandırması.
    - **Repositories**: Domain arayüzlerinin somut implementasyonları.
    - **Services**: JWT token üretimi ve şifre hashing işlemleri.
4.  **CleanArcOgrNotSis.Presentation**: API katmanıdır.
    - **Controllers**: Dış dünyadan gelen HTTP isteklerini karşılayan endpoint'ler.
    - **Program.cs**: Servis kayıtları ve middleware konfigürasyonları.

## 🔑 Kimlik Doğrulama ve Yetkilendirme

Sistemde **Admin**, **Öğretmen** ve **Öğrenci** olmak üzere üç farklı rol bulunmaktadır:
- **Admin**: Tam yetkiye sahiptir. Kullanıcı, öğretmen ve ders yönetimi yapabilir.
- **Öğretmen**: Ders ve not girişi yapabilir.
- **Öğrenci**: Kendi bilgilerini ve notlarını görüntüleyebilir.

Şifreleri veritabanında güvenli bir şekilde saklamak için **BCrypt** algoritması kullanılmıştır.

## ⚙️ Kurulum ve Çalıştırma

1.  Bilgisayarınızda .NET SDK (8.0+) yüklü olduğundan emin olun.
2.  Kök dizindeki `appsettings.json` dosyasındaki `DefaultConnection` kısmına PostgreSQL bağlantı dizeinizi (Connection String) girin.
3.  Terminalde Presentation projesinin içine gidin veya root dizinden çalıştırın:
    ```bash
    dotnet run --project CleanArcOgrNotSis.Presentation
    ```
4.  Tarayıcıdan `http://localhost:XXXX/swagger` adresine giderek API'yi test edin.

## 📝 Ek Notlar

- Uygulama ilk çalıştığında otomatik olarak **SeedData** oluşturacak şekilde yapılandırılmıştır.
- Swagger üzerinden işlem yaparken JWT Token kullanmak için "Authorize" butonunu kullanabilirsiniz.

---
*Bu proje, profesyonel bir yazılım geliştirme sürecini simüle etmek amacıyla uçtan uca prensiplere bağlı kalınarak geliştirilmiştir.*
