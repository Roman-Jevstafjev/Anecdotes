using Arch.EntityFrameworkCore.UnitOfWork;
using Ardalis.Result;
using AutoMapper;
using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.ViewModels;
using Jevstafjev.Anecdotes.Web.Application.Services;
using MediatR;
using System.Security.Claims;

namespace Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.Queries
{
    public record AnecdoteDeleteRequest(Guid AnecdoteId, ClaimsPrincipal User) : IRequest<Result<AnecdoteViewModel>>;

    public class AnecdoteDeleteRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ITagService tagService)
        : IRequestHandler<AnecdoteDeleteRequest, Result<AnecdoteViewModel>>
    {
        public async Task<Result<AnecdoteViewModel>> Handle(AnecdoteDeleteRequest request, CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Anecdote>();

            var entity = await repository.GetFirstOrDefaultAsync(
                predicate: x => x.Id == request.AnecdoteId,
                disableTracking: false);
            if (entity is null)
            {
                return Result<AnecdoteViewModel>.NotFound("Anecdote is not found");
            }

            repository.Delete(entity);
            await unitOfWork.SaveChangesAsync();

            var mapped = mapper.Map<AnecdoteViewModel>(entity);
            return Result<AnecdoteViewModel>.Success(mapped);
        }
    }
}
