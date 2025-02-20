using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Endpoint;

namespace WebApplication2.Features.Users;

public class RemoveUser
{
    public record RemoveUserRequest(int UserId): IRequest<IResult>;
    
    
    public class RemoveUserHandler : IRequestHandler<RemoveUserRequest, IResult>
    {
        private readonly BookListingDbContext _context;
    
        public RemoveUserHandler(BookListingDbContext context)
        {
            _context = context;
        }
        
        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
        
                app.MapDelete("/users/{userId}", async ([FromRoute] int userId, IMediator mediator) =>
                {
                    var response = await mediator.Send(new RemoveUserRequest(userId));
                    return Results.Ok(response);
                }).WithTags("Users");
            }
        }
        
        public async Task<IResult> Handle(RemoveUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.ApiUsers.FindAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return Results.NotFound();
            }
            _context.ApiUsers.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
            return Results.NoContent();
        }
    }
}