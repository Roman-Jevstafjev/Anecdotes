using Jevstafjev.Anecdotes.Infrastructure;
using Jevstafjev.Anecdotes.Web.Definitions.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jevstafjev.Anecdotes.Web.Definitions.DbContext
{
    public class DbContextDefinition : AppDefinition
    {
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString(nameof(ApplicationDbContext));
                options.UseSqlServer(connectionString);
            });
        }
    }
}
