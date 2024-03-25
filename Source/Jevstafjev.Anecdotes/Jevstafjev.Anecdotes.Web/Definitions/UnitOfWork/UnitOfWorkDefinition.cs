using Arch.EntityFrameworkCore.UnitOfWork;
using Jevstafjev.Anecdotes.Infrastructure;
using Jevstafjev.Anecdotes.Web.Definitions.Base;

namespace Jevstafjev.Anecdotes.Web.Definitions.UnitOfWork
{
    public class UnitOfWorkDefinition : AppDefinition
    {
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddUnitOfWork<ApplicationDbContext>();
        }
    }
}
