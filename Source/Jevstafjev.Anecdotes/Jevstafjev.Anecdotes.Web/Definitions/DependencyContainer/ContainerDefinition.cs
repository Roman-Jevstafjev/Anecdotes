using Jevstafjev.Anecdotes.Web.Application.Services;
using Jevstafjev.Anecdotes.Web.Definitions.Base;

namespace Jevstafjev.Anecdotes.Web.Definitions.DependencyContainer
{
    public class ContainerDefinition : AppDefinition
    {
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ITagService, TagService>();
        }
    }
}
