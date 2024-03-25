using Jevstafjev.Anecdotes.Domain.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jevstafjev.Anecdotes.Infrastructure.Base
{
    public abstract class DbContextBase : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbContextBase(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public override int SaveChanges()
        {
            DbSaveChanges();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            DbSaveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DbSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            DbSaveChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void DbSaveChanges()
        {
            const string DefaultUserName = "Anonymous";

            var createdEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);
            foreach (var entry in createdEntries)
            {
                var auditable = entry.Entity as IAuditable;
                if (auditable is null)
                {
                    continue;
                }

                auditable.CreatedAt = DateTime.UtcNow;
                auditable.CreatedBy ??= DefaultUserName;
                auditable.UpdatedAt = DateTime.UtcNow;
                auditable.UpdatedBy = auditable.CreatedBy;
            }

            var modifiedEntries = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);
            foreach (var entry in modifiedEntries)
            {
                var auditable = entry.Entity as IAuditable;
                if (auditable is null)
                {
                    continue;
                }

                auditable.UpdatedAt = DateTime.UtcNow;
                auditable.UpdatedBy ??= DefaultUserName;
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
