using Jevstafjev.Anecdotes.Domain.Base;

namespace Jevstafjev.Anecdotes.Domain
{
    public class Tag : Identity
    {
        public string Name { get; set; } = null!;

        public virtual List<Anecdote>? Anecdotes { get; set; }
    }
}
