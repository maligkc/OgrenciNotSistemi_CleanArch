using System.Text;
using CleanArcOgrNotSis.Application.Behaviors;
using CleanArcOgrNotSis.Application.DTOs;
using CleanArcOgrNotSis.Application.Interfaces;
using CleanArcOgrNotSis.Application.Mappings;
using CleanArcOgrNotSis.Domain.Interfaces;
using CleanArcOgrNotSis.Infrastructure.Context;
using CleanArcOgrNotSis.Infrastructure.Repositories;
using CleanArcOgrNotSis.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // Controller sınıflarını kullanabilmek için ekler
builder.Services.AddEndpointsApiExplorer(); // Swaggerın endpointleri keşfetmesi için gerekli
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Öğrenci Not Sistemi API",
        Version = "v1"
    });
    
    // JWT Bearer tanımı — Swagger'da "Authorize 🔒" butonu çıkar
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT token giriniz. Sadece token'ı yapıştırın, 'Bearer ' ön ekini eklemenize gerek yok."
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    // Tüm endpoint'lerde global olarak uygula
    c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
    
}); // Swagger UI ve dokümantasyonu oluşturur

builder.Services.AddDbContext<OgrenciNotSistemiDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});// EF Core'u Postgresql'e ile bağlar, connection string appsettings.json'dan okunur

builder.Services.AddScoped<IOgrenciRepository, OgrenciRepository>(); // IOgrenciRepository istendiğinde OgrenciRepository'yi kullan (her request'te yeni instance)
builder.Services.AddScoped<IDersRepository, DersRepositories>();     // IDersRepository istendiğinde DersRepository'yi kullan
builder.Services.AddScoped<INotRepository, NotRepository>();         // INotRepository istendiğinde NotRepository'yi kullan

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IOgretmenRepository, OgretmenRepository>();
builder.Services.AddScoped<IKullaniciRepository, KullaniciRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ISifreService, SifreService>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(OgrenciDto).Assembly);

    // Her command çalışmadan önce ValidationBehavior devreye girer
    
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
// MediatR'ı ekler, Application katmanındaki tüm Handler'ları otomatik bulur




// ─── FluentValidation (Assembly taraması) ─────────────────────
// Validators klasöründeki tüm validator'ları otomatik bulur ve kaydeder
builder.Services.AddValidatorsFromAssembly(typeof(OgrenciDto).Assembly);




builder.Services.AddAutoMapper(typeof(OgrenciMappingProfile)); 
// AutoMapper'ı ekler, OgrenciMappingProfile'daki mapping kurallarını yükler


var jwtAyarlari = builder.Configuration.GetSection("JwtAyarlari");
var gizliAnahtar = Encoding.UTF8.GetBytes(jwtAyarlari["GizliAnahtar"]!);


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,            // Token süresi dolmuş mu kontrol eder
            ValidateIssuerSigningKey = true,    // İmza doğrulaması
            ValidIssuer = jwtAyarlari["Issuer"],
            ValidAudience = jwtAyarlari["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(gizliAnahtar)
        };
    });

builder.Services.AddAuthorization();



var app = builder.Build(); // Tüm servisler kaydedildi, uygulama oluşturuldu


if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger JSON endpoint'ini aktif eder (sadece geliştirme ortamında)
    app.UseSwaggerUI(); // Swagger arayüzünü aktif eder → /swagger adresinden erişilir
    
    
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<OgrenciNotSistemiDbContext>();
        db.Database.EnsureCreated();
    }

    await DbSeeder.SeedAsync(app.Services);
}

// using (var scope = app.Services.CreateAsyncScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<OgrenciNotSistemiDbContext>();
//     db.Database.EnsureCreated(); // Uygulama başlarken database yoksa otomatik oluşturur
// }



app.UseHttpsRedirection();  // HTTP isteklerini otomatik olarak HTTPS'e yönlendirir

app.UseAuthentication();    // JWT token doğrulama — UseAuthorization'dan ÖNCE gelmeli

app.UseAuthorization();     // Yetkilendirme middleware'ini ekler (token, rol kontrolü vs.)

app.MapControllers();       // Controller'lardaki route'ları aktif eder


await app.RunAsync();     // Uygulamayı başlatır, istekleri dinlemeye başlar
