using Jevstafjev.Anecdotes.Infrastructure.DatabaseInitialization;
using Jevstafjev.Anecdotes.Web.Definitions.Base;

namespace Jevstafjev.Anecdotes.Web.Definitions.DatabaseInitialization
{
    public class DatabaseInitializationDefinition : AppDefinition
    {
        public override void ConfigureApplication(WebApplication app)
        {
            DatabaseInitializer.SeedUsers(app.Services).Wait();
        }
    }
}
