using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Endpoint;
using WebApplication2.Models.User;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Features.Users;

public class GetUser
{
    public record GetUserRequest(int UserId): IRequest<UserDto>;


    public class GetUserHandler : IRequestHandler<GetUserRequest, UserDto>
    {
        private readonly BookListingDbContext _context;
        private readonly IMapper _mapper;

        public GetUserHandler(BookListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {

                app.MapGet("/users/{userId}", async ([FromRoute] int userId, IMediator mediator) =>
                {
                    var response = await mediator.Send(new GetUserRequest(userId));
                    return Results.Ok(response);
                }).WithTags("Users");
            }
        }

        public async Task<UserDto> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.ApiUsers.Include(u => u.Books)
                .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            var mappedUser = _mapper.Map<UserDto>(user);
            return mappedUser;
        }
    }
}