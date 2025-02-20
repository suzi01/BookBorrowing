using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Endpoint;

namespace WebApplication2.Features.Books;

public class DeleteBook
{
    public record DeleteBookRequest(int BookId): IRequest<IResult>;
    
    
    public class DeleteBookHandler : IRequestHandler<DeleteBookRequest, IResult>
    {
        private readonly BookListingDbContext _context;
    
        public DeleteBookHandler(BookListingDbContext context)
        {
            _context = context;
        }
        
        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
        
                app.MapDelete("/books/{bookId}", async ([FromRoute] int bookId, IMediator mediator) =>
                {
                    var response = await mediator.Send(new DeleteBookRequest(bookId));
                    return Results.Ok(response);
                }).WithTags("Books");
            }
        }
        
        public async Task<IResult> Handle(DeleteBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(request.BookId, cancellationToken);
            if (book == null)
            {
                return Results.NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync(cancellationToken);
            return Results.NoContent();
        }
    }
}