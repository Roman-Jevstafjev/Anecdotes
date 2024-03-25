using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Web.Application.Messaging.TagsMessages.Queries;
using Jevstafjev.Anecdotes.Web.Definitions.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jevstafjev.Anecdotes.Web.Endpoints
{
    public class TagEndpoints : AppDefinition
    {
        public override void ConfigureApplication(WebApplication app)
        {
            app.MapTagEndpoints();
        }
    }

    internal static class TagEndpointsExtensions
    {
        public static void MapTagEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/tags/").WithTags(nameof(Tag));

            group.MapGet("get-list", async ([FromServices] IMediator mediator, HttpContext context) =>
                await mediator.Send(new TagGetListRequest(), context.RequestAborted))
                .Produces(200)
                .WithOpenApi();
        }
    }
}
