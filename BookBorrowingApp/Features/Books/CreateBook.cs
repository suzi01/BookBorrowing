using AutoMapper;
using FluentValidation;
using MediatR;
using WebApplication2.Data;
using WebApplication2.Endpoint;
using WebApplication2.Models.Book;

namespace WebApplication2.Features.Books;

public static class CreateBook
{
    public record CreateBookRequest(string Title, string Author, string Publisher):IRequest<CreateBookResponse>;
    public record CreateBookResponse(int Id, string Title, string Author, string Publisher);

    public class CreateBookValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required in order to create book");
            RuleFor(x => x.Author).NotEmpty().WithMessage("Author is required in order to create book");
            RuleFor(x => x.Publisher).NotEmpty().WithMessage("Publisher is required in order to create book");
        }
    }
    
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/books", async (CreateBookRequest request, IMediator mediator) =>
            {
                try
                {
                    var response = await mediator.Send(request);
                    // return Results.Ok(response);
                    return Results.Created(new Uri("/books", UriKind.Relative ), response);
                }
                catch (ValidationException exception)
                {
                    return Results.BadRequest(exception.Errors);
                }
               
            }).WithTags("Books");
           
        }
    }

    public class CreateBookHandler : IRequestHandler<CreateBookRequest, CreateBookResponse>
    {
        private readonly BookListingDbContext _context;
        private readonly IMapper _mapper;


        public CreateBookHandler(BookListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreateBookResponse> Handle(CreateBookRequest createBookRequest, CancellationToken cancellationToken)
        {
            var bookDto = _mapper.Map<BookDTO>(createBookRequest);
            var book = _mapper.Map<Book>(bookDto);
            _context.Books.Add(book);
            await _context.SaveChangesAsync(cancellationToken);
            
            return new CreateBookResponse(book.BookId, book.Title, book.Author, book.Publisher);
        }
    }
}