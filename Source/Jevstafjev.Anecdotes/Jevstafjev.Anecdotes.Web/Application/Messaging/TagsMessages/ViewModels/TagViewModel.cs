namespace Jevstafjev.Anecdotes.Web.Application.Messaging.TagsMessages.ViewModels
{
    public class TagViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public int AnecdotesCount { get; set; }
    }
}
