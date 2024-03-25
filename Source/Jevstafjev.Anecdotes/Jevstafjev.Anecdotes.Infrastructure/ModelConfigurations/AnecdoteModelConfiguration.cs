using Jevstafjev.Anecdotes.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jevstafjev.Anecdotes.Infrastructure.ModelConfigurations
{
    public class AnecdoteModelConfiguration : IEntityTypeConfiguration<Anecdote>
    {
        public void Configure(EntityTypeBuilder<Anecdote> builder)
        {
            builder.ToTable("Anecdotes");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.CreatedAt).HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc)).IsRequired();
            builder.Property(x => x.CreatedBy).HasMaxLength(256).IsRequired();
            builder.Property(x => x.UpdatedAt).HasConversion(v => v!.Value, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            builder.Property(x => x.UpdatedBy).HasMaxLength(256);

            builder.HasMany(x => x.Tags);
        }
    }
}
