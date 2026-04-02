using CleanArcOgrNotSis.Application.DTOs;
using FluentValidation;

namespace CleanArcOgrNotSis.Application.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email alanı zorunludur")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz")
            .MaximumLength(200).WithMessage("Email en fazla 200 karakter olabilir");

        RuleFor(x => x.Sifre)
            .NotEmpty().WithMessage("Şifre alanı zorunludur")
            .MinimumLength(5).WithMessage("Şifre en az 5 karakter olabilir");

        RuleFor(x => x.SifreTekrar)
            .NotEmpty().WithMessage("Şifre tekrar alanı zorunludur")
            .Equal(x => x.Sifre).WithMessage("Şifreler eşleşmiyor");
        RuleFor(x => x.Rol)
            .NotEmpty().WithMessage("Rol alanı zorunludur")
            .Must(rol => new[] { "Ogrenci", "Ogretmen", "Admin" }.Contains(rol))
            .WithMessage("Rol 'Ogrenci', 'Ogretmen' veya 'Admin' olmalıdır");

    }
}