using FluentValidation;
using MediatR;

namespace CleanArcOgrNotSis.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        
        if (!_validators.Any()) // Bu komut için kayıtlı validator var mı?
            return await next(); // Yoksa direkt devam et

        var context = new ValidationContext<TRequest>(request);

        // Tüm validator'ları paralel çalıştır
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        
        // Hataları topla
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        
        // Hata varsa fırlat → Controller catch bloğu yakalar
        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        return await next(); // Hata yoksa handler'a geç

    }
}