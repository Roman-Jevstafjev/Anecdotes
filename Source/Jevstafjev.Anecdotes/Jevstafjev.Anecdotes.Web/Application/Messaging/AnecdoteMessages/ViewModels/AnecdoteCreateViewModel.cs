namespace Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.ViewModels
{
    public class AnecdoteCreateViewModel
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string Tags { get; set; } = null!;
    }
}
