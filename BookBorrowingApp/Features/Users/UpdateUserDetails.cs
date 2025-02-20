using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Endpoint;
using WebApplication2.Models.User;

namespace WebApplication2.Features.Users;

public class UpdateUserDetails
{
    public record UpdateUserDetailsRequest(int UserId, UpdateUserDto UpdateUserDto): IRequest<IResult>;
    
    public class UpdateUserDetailsValidator : AbstractValidator<UpdateUserDetailsRequest>
    {
        public UpdateUserDetailsValidator()
        {
            RuleFor(x => x.UpdateUserDto.FirstName).NotEmpty().WithMessage("First name cannot be empty");
            RuleFor(x => x.UpdateUserDto.LastName).NotEmpty().WithMessage("Last name cannot be empty");
            RuleFor(x => x.UpdateUserDto.Email).EmailAddress().WithMessage("Email address is required");
        }
    }
    
    public class UpdateUserDetailsHandler : IRequestHandler<UpdateUserDetailsRequest, IResult>
    {
        private readonly BookListingDbContext _context;
        private readonly IMapper _mapper;

        public UpdateUserDetailsHandler(BookListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
        
                app.MapPut("/users/{userId}", async ([FromRoute] int userId , [FromBody] UpdateUserDto updateUserDto, IMediator mediator) =>
                {
                    var response = await mediator.Send(new UpdateUserDetailsRequest(userId, updateUserDto));
                    return Results.Ok(response);
                }).WithTags("Users");
            }
        }
        
        public async Task<IResult> Handle(UpdateUserDetailsRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.ApiUsers.FindAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return Results.NotFound();
            }
            
            
            _mapper.Map(request.UpdateUserDto, user);
            
            await _context.SaveChangesAsync(cancellationToken);
            return Results.Ok(_mapper.Map<UserDto>(user));
        }
    }
}