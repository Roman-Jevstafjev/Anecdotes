using AutoMapper;
using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Web.Application.Messaging.TagsMessages.ViewModels;

namespace Jevstafjev.Anecdotes.Web.Application.Messaging.TagsMessages
{
    public class TagMapperConfiguration : Profile
    {
        public TagMapperConfiguration()
        {
            CreateMap<Tag, TagViewModel>()
                .ForMember(x => x.AnecdotesCount, o => o.MapFrom(x => x.Anecdotes!.Count));
        }
    }
}
