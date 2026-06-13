using System.ComponentModel.DataAnnotations;
using CinemaReservation.Catalog.Application.Common.Validation;
using MediatR;

namespace CinemaReservation.Catalog.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(request);
        var isValid = Validator.TryValidateObject(request, context, validationResults, true);

        if (!isValid)
        {
            var errors = validationResults.Select(result =>
            {
                var field = result.MemberNames.FirstOrDefault() ?? typeof(TRequest).Name;
                var code = string.IsNullOrWhiteSpace(result.ErrorMessage)
                    ? $"ERR_INVALID_{field.ToUpperInvariant()}"
                    : result.ErrorMessage!;

                return new ValidationFailureItem(field, code, result.ErrorMessage);
            });

            throw new RequestValidationException(errors);
        }

        return next();
    }
}
