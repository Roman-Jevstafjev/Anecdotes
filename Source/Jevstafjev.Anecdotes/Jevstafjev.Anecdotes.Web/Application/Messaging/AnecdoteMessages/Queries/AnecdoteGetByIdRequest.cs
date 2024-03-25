using Arch.EntityFrameworkCore.UnitOfWork;
using Ardalis.Result;
using AutoMapper;
using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.ViewModels;
using MediatR;
using System.Security.Claims;

namespace Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.Queries
{
    public record AnecdoteGetByIdRequest(Guid AnecdoteId, ClaimsPrincipal User) : IRequest<Result<AnecdoteViewModel>>;

    public class AnecdoteGetByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<AnecdoteGetByIdRequest, Result<AnecdoteViewModel>>
    {
        public async Task<Result<AnecdoteViewModel>> Handle(AnecdoteGetByIdRequest request, CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Anecdote>();

            var entity = await repository.GetFirstOrDefaultAsync(
                    predicate: x => x.Id == request.AnecdoteId);
            if (entity is null)
            {
                return Result.NotFound("Anecdote is not found");
            }

            var mapped = mapper.Map<AnecdoteViewModel>(entity);
            return Result<AnecdoteViewModel>.Success(mapped);
        }
    }
}
