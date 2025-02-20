using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Endpoint;
using WebApplication2.Models.Book;

namespace WebApplication2.Features.Books;

public class UpdateBook
{
    public record UpdateBookRequest(int BookId, BookDTO BookDto): IRequest<IResult>;
    
    // public class UpdateBookValidator : AbstractValidator<UpdateBookRequest>
    // {
    //     public UpdateBookValidator()
    //     {
    //         RuleFor(x => x.BookDto.Title).NotEmpty().WithMessage("Title is required");
    //         RuleFor(x => x.BookDto.Author).NotEmpty().WithMessage("Author is required");
    //         RuleFor(x => x.BookDto.Publisher).NotEmpty().WithMessage("Publisher is required");
    //     }
    // }
    
    public class UpdateBookHandler : IRequestHandler<UpdateBookRequest, IResult>
    {
        private readonly BookListingDbContext _context;
        private readonly IMapper _mapper;

        public UpdateBookHandler(BookListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
        
                app.MapPut("/books/{bookId}", async ([FromRoute] int bookId , [FromBody] BookDTO bookDto, IMediator mediator) =>
                {
                    var response = await mediator.Send(new UpdateBookRequest(bookId, bookDto));
                    return Results.Ok(response);
                }).WithTags("Books");
            }
        }
        
        public async Task<IResult> Handle(UpdateBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(request.BookId, cancellationToken);
            if (book == null)
            {
                return Results.NotFound();
            }
            
            _mapper.Map(request.BookDto, book);
            
            await _context.SaveChangesAsync(cancellationToken);
            return Results.Ok(_mapper.Map<BookDTO>(book));
        }
    }
}