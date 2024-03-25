using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.Queries;
using Jevstafjev.Anecdotes.Web.Application.Messaging.AnecdoteMessages.ViewModels;
using Jevstafjev.Anecdotes.Web.Definitions.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jevstafjev.Anecdotes.Web.Endpoints
{
    public class AnecdoteEndpoints : AppDefinition
    {
        public override void ConfigureApplication(WebApplication app)
        {
            app.MapAnecdoteEndpoints();
        }
    }
     
    internal static class AnecdoteEndpointsExtensions
    {
        public static void MapAnecdoteEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/anecdotes/").WithTags(nameof(Anecdote));

            group.MapGet("{id:guid}", async ([FromServices] IMediator mediator, Guid id, HttpContext context) =>
                await mediator.Send(new AnecdoteGetByIdRequest(id, context.User), context.RequestAborted))
                .Produces(200)
                .WithOpenApi();

            group.MapGet("paged/{pageIndex:int}", async ([FromServices] IMediator mediator,
                HttpContext context,
                int pageIndex,
                int pageSize = 10) =>
                await mediator.Send(new AnecdoteGetPagedRequest(pageIndex, pageSize, context.User), context.RequestAborted))
                .Produces(200)
                .WithOpenApi();

            group.MapGet("find-by-tag/{tag}", async ([FromServices] IMediator mediator,
                HttpContext context,
                string tag,
                int pageIndex = 0,
                int pageSize = 10) =>
                await mediator.Send(new AnecdoteFindByTagRequest(tag, pageIndex, pageSize, context.User), context.RequestAborted));

            group.MapGet("newest", async ([FromServices] IMediator mediator, HttpContext context, int total = 10) =>
                await mediator.Send(new AnecdoteGetNewestRequest(total), context.RequestAborted))
                .Produces(200)
                .WithOpenApi();

            group.MapGet("random", async ([FromServices] IMediator mediator, HttpContext context) =>
                await mediator.Send(new AnecdoteGetRandomRequest(), context.RequestAborted))
                .Produces(200)
                .WithOpenApi();

            group.MapGet("random-list", async ([FromServices] IMediator mediator, HttpContext context, int total = 10) =>
                await mediator.Send(new AnecdoteGetRandomListRequest(total), context.RequestAborted))
                .Produces(200)
                .WithOpenApi();

            group.MapPost("create", async ([FromServices] IMediator mediator, [FromBody] AnecdoteCreateViewModel model, HttpContext context) =>
                await mediator.Send(new AnecdoteCreateRequest(model, context.User), context.RequestAborted))
                .RequireAuthorization(AppData.DefaultPolicyName)
                .RequireAuthorization(x => x.RequireRole(AppData.AdministratorRoleName))
                .Produces(200)
                .ProducesProblem(401)
                .WithOpenApi();

            group.MapGet("get-for-update/{id:guid}", async ([FromServices] IMediator mediator, Guid id, HttpContext context) =>
                await mediator.Send(new AnecdoteGetForUpdateRequest(id, context.User), context.RequestAborted))
                .RequireAuthorization(AppData.DefaultPolicyName)
                .RequireAuthorization(x => x.RequireRole(AppData.AdministratorRoleName))
                .Produces(200)
                .ProducesProblem(401)
                .WithOpenApi();

            group.MapPut("update", async ([FromServices] IMediator mediator, [FromBody] AnecdoteUpdateViewModel model, HttpContext context) =>
                await mediator.Send(new AnecdoteUpdateRequest(model, context.User), context.RequestAborted))
                .RequireAuthorization(AppData.DefaultPolicyName)
                .RequireAuthorization(x => x.RequireRole(AppData.AdministratorRoleName))
                .Produces(200)
                .ProducesProblem(401)
                .WithOpenApi();

            group.MapDelete("delete/{id}", async ([FromServices] IMediator mediator, Guid id, HttpContext context) =>
                await mediator.Send(new AnecdoteDeleteRequest(id, context.User), context.RequestAborted))
                .RequireAuthorization(AppData.DefaultPolicyName)
                .RequireAuthorization(x => x.RequireRole(AppData.AdministratorRoleName))
                .Produces(200)
                .ProducesProblem(401)
                .WithOpenApi();
        }
    }
}
