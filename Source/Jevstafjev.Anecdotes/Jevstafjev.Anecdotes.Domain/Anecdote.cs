using Jevstafjev.Anecdotes.Domain.Base;

namespace Jevstafjev.Anecdotes.Domain
{
    public class Anecdote : Auditable
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public virtual List<Tag>? Tags { get; set; }
    }
}
