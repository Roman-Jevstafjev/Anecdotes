using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;

namespace Jevstafjev.Anecdotes.Infrastructure
{
    public class ApplicationDbContext : DbContextBase
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Anecdote> Anecdotes { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Anecdote>().Navigation(x => x.Tags).AutoInclude();
        }
    }
}
