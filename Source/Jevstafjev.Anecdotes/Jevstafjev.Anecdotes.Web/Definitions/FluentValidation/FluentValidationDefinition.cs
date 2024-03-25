using FluentValidation;
using Jevstafjev.Anecdotes.Web.Definitions.Base;
using Microsoft.AspNetCore.Mvc;

namespace Jevstafjev.Anecdotes.Web.Definitions.FluentValidation
{
    public class FluentValidationDefinition : AppDefinition
    {
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        }
    }
}
