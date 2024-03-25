using Arch.EntityFrameworkCore.UnitOfWork;
using Ardalis.Result;
using AutoMapper;
using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.ViewModels;
using MediatR;

namespace Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.Queries
{
    public record AnecdoteGetRandomRequest : IRequest<Result<AnecdoteViewModel>>;

    public class AnecdoteGetRandomRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<AnecdoteGetRandomRequest, Result<AnecdoteViewModel>>
    {
        public Task<Result<AnecdoteViewModel>> Handle(AnecdoteGetRandomRequest request, CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Anecdote>();

            var entity = repository.GetAll()
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefault();

            if (entity is null)
            {
                return Task.FromResult(Result<AnecdoteViewModel>.NotFound("Anecdote is not found"));
            }

            var mapped = mapper.Map<AnecdoteViewModel>(entity);
            return Task.FromResult(Result.Success(mapped));
        }
    }
}
