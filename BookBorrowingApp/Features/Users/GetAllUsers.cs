using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Endpoint;
using WebApplication2.Models.User;

namespace WebApplication2.Features.Users;

public class GetAllUsers
{
    public record GetAllUsersRequest() : IRequest<IEnumerable<UserDto>>;
    

    public class GetAllUsersHandler : IRequestHandler<GetAllUsersRequest, IEnumerable<UserDto>>
    {
        private readonly BookListingDbContext _context;
        private readonly IMapper _mapper;
        
        public GetAllUsersHandler(BookListingDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            _mapper = mapper;
        }

        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapGet("/users", async ([FromServices]IMediator mediator) =>
                {   
                    var result = await mediator.Send(new GetAllUsersRequest());
                    return Results.Ok(result);
                }).WithTags("Users");
            }
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
        {
            var users = await _context.ApiUsers.Include(u => u.Books).ToListAsync(cancellationToken);
                
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}