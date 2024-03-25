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
    public record AnecdoteGetPagedRequest(int PageIndex, int PageSize, ClaimsPrincipal User) : IRequest<PagedResult<List<AnecdoteViewModel>>>;

    public class AnecdoteGetPagedRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<AnecdoteGetPagedRequest, PagedResult<List<AnecdoteViewModel>>>
    {
        public async Task<PagedResult<List<AnecdoteViewModel>>> Handle(AnecdoteGetPagedRequest request, CancellationToken cancellationToken)
        {
            var repository = unitOfWork.GetRepository<Anecdote>();
            var pagedList = await repository.GetPagedListAsync(
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
