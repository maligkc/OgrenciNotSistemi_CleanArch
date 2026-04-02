# Öğrenci Not Sistemi (Clean Architecture & CQRS)

Bu proje, modern yazılım mimarisi prensipleri kullanılarak geliştirilmiş kapsamlı bir **Öğrenci Not Sistemi API**'sidir. Proje, sürdürülebilirlik, test edilebilirlik ve esneklik odaklı **Clean Architecture** üzerine inşa edilmiştir.

## 🚀 Teknolojiler ve Yaklaşımlar

- **.NET 10**: En güncel web framework sürümü.
- **Clean Architecture**: Katmanlı mimari (Domain, Application, Infrastructure, Presentation).
- **CQRS (MediatR)**: Komut ve sorguların ayrıştırılması.
- **Entity Framework**: Veritabanı yönetim katmanı.
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

## 🌟 Öne Çıkan Özellikler

- **JWT & Role-Based Auth**: Sistemde Admin, Öğretmen ve Öğrenci rolleri tanımlıdır. Her rol sadece kendi yetkisi dahilindeki alanlara (Swagger'da asma kilit ile görünen alanlar) erişebilir.
- **PostgreSQL Entegrasyonu**: Performanslı ve açık kaynak bir veritabanı tercihi yapılmıştır.
- **Automatic Seeding**: Uygulama ilk ayağa kalktığında DbSeeder sınıfı sayesinde varsayılan Admin kullanıcısı (admin / admin123) ve örnek veriler otomatik olarak oluşturulur.
- **Swagger UI**: API'nin tüm uç noktalarını (Endpoints) test edebileceğiniz, JWT token yapıştırabileceğiniz interaktif bir dökümantasyon sayfası sunar.

## API Endpointleri

### 🔐 Auth
- POST /api/auth/kayit → Yeni kullanıcı kaydı
- POST /api/auth/giris → Kullanıcı girişi (Token üretimi)

### 🎓 Ogrenci
- GET /api/ogrenci → Tüm öğrencileri listele (Admin, Öğretmen)
- GET /api/ogrenci/{id} → ID bazlı öğrenci getir (Admin, Öğretmen, Öğrenci)
- POST /api/ogrenci → Yeni öğrenci ekle (Admin)
- PUT /api/ogrenci/{id} → Öğrenci güncelle (Admin, Öğretmen)
- DELETE /api/ogrenci/{id} → Öğrenci sil (Admin)

### 👨‍🏫 Ogretmen
- GET /api/ogretmen → Tüm öğretmenleri listele (Admin)
- GET /api/ogretmen/{id} → ID bazlı öğretmen getir (Admin, Öğretmen)
- POST /api/ogretmen → Yeni öğretmen ekle (Admin)
- PUT /api/ogretmen/{id} → Öğretmen güncelle (Admin, Öğretmen)
- DELETE /api/ogretmen/{id} → Öğretmen sil (Admin)

### 📚 Dersler
- GET /api/dersler → Tüm dersleri listele (Tüm roller)
- GET /api/dersler/{id} → ID bazlı ders getir (Tüm roller)
- POST /api/dersler → Yeni ders ekle (Admin, Öğretmen)
- PUT /api/dersler/{id} → Ders güncelle (Admin, Öğretmen)
- DELETE /api/dersler/{id} → Ders sil (Admin, Öğretmen)

### 📝 Notlar
- GET /api/notlar → Tüm notları listele (Admin, Öğretmen)
- GET /api/notlar/{id} → ID bazlı not getir (Tüm roller)
- GET /api/notlar/ogrenci/{ogrenciId} → Öğrenciye ait notları getir (Tüm roller)
- GET /api/notlar/ders/{dersId} → Derse ait notları getir (Admin, Öğretmen)
- POST /api/notlar → Not girişi yap (Admin, Öğretmen)
- PUT /api/notlar/{id} → Not güncelle (Admin, Öğretmen)
- DELETE /api/notlar/{id} → Not sil (Admin, Öğretmen)


## ⚙️ Kurulum ve Çalıştırma

1.  Bilgisayarınızda .NET 10 yüklü olduğundan emin olun.
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
