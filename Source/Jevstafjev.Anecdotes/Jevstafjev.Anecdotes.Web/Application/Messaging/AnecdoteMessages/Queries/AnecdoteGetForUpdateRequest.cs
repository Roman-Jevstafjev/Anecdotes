using Arch.EntityFrameworkCore.UnitOfWork;
using Ardalis.Result;
using AutoMapper;
using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.ViewModels;
using MediatR;
using System.Security.Claims;

namespace Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.Queries
{
    public record AnecdoteGetForUpdateRequest(Guid AnecdoteId, ClaimsPrincipal User) : IRequest<Result<AnecdoteUpdateViewModel>>;

    public class AnecdoteGetForUpdateRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<AnecdoteGetForUpdateRequest, Result<AnecdoteUpdateViewModel>>
    {
        public async Task<Result<AnecdoteUpdateViewModel>> Handle(AnecdoteGetForUpdateRequest request, CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Anecdote>();
            
            var entity = await repository.GetFirstOrDefaultAsync(
                    predicate: x => x.Id == request.AnecdoteId);
            if (entity is null)
            {
                return Result.NotFound("Anecdote is not found");
            }

            var mapped = mapper.Map<AnecdoteUpdateViewModel>(entity);
            return Result<AnecdoteUpdateViewModel>.Success(mapped);
        }
    }
}
