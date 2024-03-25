using Arch.EntityFrameworkCore.UnitOfWork;
using Ardalis.Result;
using AutoMapper;
using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.ViewModels;
using MediatR;

namespace Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.Queries
{
    public record AnecdoteGetNewestRequest(int Total) : IRequest<Result<List<AnecdoteViewModel>>>;

    public class AnecdoteGetNewestRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<AnecdoteGetNewestRequest, Result<List<AnecdoteViewModel>>>
    {
        public Task<Result<List<AnecdoteViewModel>>> Handle(AnecdoteGetNewestRequest request, CancellationToken cancellationToken)
        {
            if (request.Total is < 5 or > 30)
            {
                return Task.FromResult(Result<List<AnecdoteViewModel>>.Invalid(
                    new ValidationError("Total can't be less than 5 and more than 30")));
            }

            var repository = unitOfWork.GetRepository<Anecdote>();

            var entities = repository.GetAll()
                .OrderByDescending(x => x.CreatedAt)
                .Take(request.Total);

            var mapped = mapper.Map<List<AnecdoteViewModel>>(entities);
            return Task.FromResult(Result.Success(mapped));
        }
    }
}
