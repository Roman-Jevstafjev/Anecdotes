namespace Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.ViewModels
{
    public class AnecdoteUpdateViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string Tags { get; set; } = null!;
    }
}
