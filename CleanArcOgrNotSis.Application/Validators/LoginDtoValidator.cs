using CleanArcOgrNotSis.Application.DTOs;
using FluentValidation;

namespace CleanArcOgrNotSis.Application.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email alanı zorunludur")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz");

        RuleFor(x => x.Sifre)
            .NotEmpty().WithMessage("Şifre alanı zorunludur");
    }
}