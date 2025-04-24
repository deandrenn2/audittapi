using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite;
using Auditt.Domain.Shared;
using Auditt.Domain.Authentications;


namespace Auditt.Application.Features.Users;

public class GetCurrentUser : ICarterModule
{
    public record GetCurrentUserResponse(int Id, string? FirstName, string? LastName, string? Email, int? IdAvatar);
    public record GetCurrentUserQuery(string Token) : IRequest<IResult>;
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/user/me", async (HttpContext req, IMediator mediator) =>
        {
            return await mediator.Send(new GetCurrentUserQuery(req.Request.Cookies["access_token"]?? ""));
        })
        .WithName(nameof(GetCurrentUser))
        .WithTags(nameof(User))
        .Produces(StatusCodes.Status200OK);
    }
    public class GetCurrentUserHandler(AppDbContext context, IManagerToken managerToken) : IRequestHandler<GetCurrentUserQuery, IResult>
    {
        public async Task<IResult> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.Token)) 
                return Results.Unauthorized();

            var email = managerToken.ValidateToken(request.Token);
            if (email == null) return Results.Unauthorized();


            var user = await context.Users.FirstOrDefaultAsync();
            if (user == null)
            {
                return Results.Ok( Result.Failure( new Error("User.GetCurrent", "No se encontró el usuario actual")));
            }
            var currentUser = new GetCurrentUserResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.IdAvatar
            );
            return Results.Ok(Result<GetCurrentUserResponse>.Success(currentUser, "Usuario actual"));
        }
    }
}