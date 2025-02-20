using AutoMapper;
using FluentValidation;
using MediatR;
using WebApplication2.Data;
using WebApplication2.Endpoint;
using WebApplication2.Models.User;

namespace WebApplication2.Features.Users;

public class CreateUser
{
    public record CreateUserRequest(string FirstName, string LastName, string Email, string Password):IRequest<CreateUserResponse>;
    
    public record CreateUserResponse(UserDto User);

    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Firstname is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Lastname is required");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty")
                .MinimumLength(6).WithMessage("Password length must be at least 6 characters")
                .MaximumLength(16).WithMessage("Password length must be no longer than 16 characters")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one upper case letter")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least one lower case")
                .Matches(@"[0-9]+").WithMessage("Password must contain number")
                .Matches(@"[\!\?\*\.]+").WithMessage("Password must contain at least one (!?*.) character");
        }
    }
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/users", async (CreateUserRequest request, IMediator mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            }).WithTags("Users");
           
        }
    }

    public class Handler : IRequestHandler<CreateUserRequest, CreateUserResponse>
    {
        private readonly BookListingDbContext _context;
        private readonly IMapper _mapper;

        public Handler(BookListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateUserResponse> Handle(CreateUserRequest createUserRequest,
            CancellationToken cancellationToken)
        {
            var newUser = _mapper.Map<ApiUser>(createUserRequest);
            _context.ApiUsers.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);
            
            return new CreateUserResponse(_mapper.Map<UserDto>(newUser));
        }
    }
}