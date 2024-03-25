using Arch.EntityFrameworkCore.UnitOfWork;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using Ardalis.Result;
using AutoMapper;
using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.ViewModels;
using MediatR;
using System.Security.Claims;

namespace Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.Queries
{
    public record AnecdoteFindByTagRequest(string Tag, int PageIndex, int PageSize, ClaimsPrincipal User) : IRequest<PagedResult<List<AnecdoteViewModel>>>;

    public class AnecdoteFindByTagRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<AnecdoteFindByTagRequest, PagedResult<List<AnecdoteViewModel>>>
    {
        public async Task<PagedResult<List<AnecdoteViewModel>>> Handle(AnecdoteFindByTagRequest request, CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Anecdote>();
            var pagedList = await repository.GetPagedListAsync(
                predicate: x => x.Tags!.Select(x => x.Name).Contains(request.Tag),
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                orderBy: x => x.OrderByDescending(x => x.CreatedAt));

            var mapped = mapper.Map<IPagedList<AnecdoteViewModel>>(pagedList);
            return new PagedResult<List<AnecdoteViewModel>>(
                new PagedInfo(mapped.PageIndex, mapped.PageSize, mapped.TotalPages, mapped.TotalCount),
                mapped.Items.ToList());
        }
    }
}
