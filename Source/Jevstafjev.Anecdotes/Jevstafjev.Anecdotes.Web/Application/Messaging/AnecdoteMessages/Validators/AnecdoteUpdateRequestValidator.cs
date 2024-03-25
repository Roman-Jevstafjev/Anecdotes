using FluentValidation;
using Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.Queries;

namespace Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.Validators
{
    public class AnecdoteUpdateRequestValidator : AbstractValidator<AnecdoteUpdateRequest>
    {
        public AnecdoteUpdateRequestValidator()
        {
            RuleFor(x => x.Model.Id).NotEmpty();
            RuleFor(x => x.Model.Title).NotEmpty().Length(2, 50);
            RuleFor(x => x.Model.Content).NotEmpty().Length(2, 10_000);
            RuleFor(x => x.Model.Tags).NotEmpty().MaximumLength(255);
        }
    }
}
