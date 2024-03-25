using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;

namespace Jevstafjev.Anecdotes.Web.Definitions.FluentValidation
{
    public class ValidatorBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationResults = validators.Select(x => x.Validate(new ValidationContext<TRequest>(request)));

            var resultErrors = validationResults.SelectMany(x => x.AsErrors()).ToList();
            var failures = validationResults
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();
            if (!failures.Any())
            {
                return next();
            }

            if (typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var invalidMethod = typeof(Result<>)
                    .MakeGenericType(resultType)
                    .GetMethod(nameof(Result<int>.Invalid), [typeof(List<ValidationError>)]);

                if (invalidMethod != null)
                {
                    return Task.FromResult((TResponse)invalidMethod.Invoke(null, new object[] { resultErrors })!);
                }
            }

            if (typeof(TResponse) == typeof(Result))
            {
                return Task.FromResult((TResponse)(object)Result.Invalid(resultErrors));
            }

            throw new ValidationException(failures);
        }
    }
}
