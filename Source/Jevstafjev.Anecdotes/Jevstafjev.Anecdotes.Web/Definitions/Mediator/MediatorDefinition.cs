using Jevstafjev.Anecdotes.Web.Definitions.Base;
using Jevstafjev.Anecdotes.Web.Definitions.FluentValidation;
using MediatR;

namespace Jevstafjev.Anecdotes.Web.Definitions.Mediator
{
    public class MediatorDefinition : AppDefinition
    {
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<Program>());
        }
    }
}
