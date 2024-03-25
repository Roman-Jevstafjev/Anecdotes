using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using AutoMapper;
using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Infrastructure;
using Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.ViewModels;
using Jevstafjev.Anecdotes.Web.Definitions.Mapping;

namespace Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages
{
    public class AnecdoteMapperConfiguration : Profile
    {
        public AnecdoteMapperConfiguration()
        {
            CreateMap<Anecdote, AnecdoteViewModel>();

            CreateMap<IPagedList<Anecdote>, IPagedList<AnecdoteViewModel>>()
                .ConvertUsing<PagedListConverter<Anecdote, AnecdoteViewModel>>();

            CreateMap<AnecdoteCreateViewModel, Anecdote>()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.Tags, o => o.Ignore())
                .ForMember(x => x.CreatedAt, o => o.Ignore())
                .ForMember(x => x.UpdatedAt, o => o.Ignore())
                .ForMember(x => x.UpdatedBy, o => o.Ignore())
                .ForMember(x => x.CreatedBy, o => o.MapFrom((_, _, _, context) => context.Items[nameof(ApplicationUser)]));

            CreateMap<AnecdoteUpdateViewModel, Anecdote>()
                .ForMember(x => x.Tags, o => o.Ignore())
                .ForMember(x => x.CreatedAt, o => o.Ignore())
                .ForMember(x => x.UpdatedAt, o => o.Ignore())
                .ForMember(x => x.UpdatedBy, o => o.MapFrom((_, _, _, context) => context.Items[nameof(ApplicationUser)]))
                .ForMember(x => x.CreatedBy, o => o.Ignore());

            CreateMap<Anecdote, AnecdoteUpdateViewModel>()
                .ForMember(x => x.Tags, o => o.MapFrom(a => string.Join(";", a.Tags!.Select(t => t.Name))));
        }
    }
}
