using Arch.EntityFrameworkCore.UnitOfWork;
using Ardalis.Result;
using AutoMapper;
using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Infrastructure;
using Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.ViewModels;
using Jevstafjev.Anecdotes.Web.Application.Services;
using MediatR;
using System.Security.Claims;

namespace Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.Queries
{
    public record AnecdoteUpdateRequest(AnecdoteUpdateViewModel Model, ClaimsPrincipal User) : IRequest<Result<AnecdoteViewModel>>;

    public class AnecdoteUpdateRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ITagService tagService)
        : IRequestHandler<AnecdoteUpdateRequest, Result<AnecdoteViewModel>>
    {
        public async Task<Result<AnecdoteViewModel>> Handle(AnecdoteUpdateRequest request, CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Anecdote>();

            var entity = await repository.GetFirstOrDefaultAsync(
                predicate: x => x.Id == request.Model.Id,
                disableTracking: false);
            if (entity is null)
            {
                return Result.NotFound("Anecdote is not found");
            }

            mapper.Map(request.Model, entity, o => o.Items[nameof(ApplicationUser)] = request.User.Identity!.Name);

            var additionResult = await tagService.AddTagsAsync(
                entity,
                request.Model.Tags.Split(new[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList());
            if (!additionResult.IsSuccess)
            {
                return Result.Invalid(new ValidationError(additionResult.Exception!.Message));
            }

            repository.Update(entity);
            await unitOfWork.SaveChangesAsync();

            var mapped = mapper.Map<AnecdoteViewModel>(entity);
            return Result<AnecdoteViewModel>.Success(mapped);
        }
    }
}
