using Jevstafjev.Anecdotes.Web.Application.Messaging.TagsMessages.ViewModels;

namespace Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.ViewModels
{
    public class AnecdoteViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public List<TagViewModel> Tags { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
