using MediatR;
using Microsoft.AspNetCore.Mvc;
using Polly;
using WebApplication2.Data;
using WebApplication2.Endpoint;


namespace WebApplication2.Features.Books;

public class BorrowBook
{
    public record BorrowBookRequest(int BookId, int UserId): IRequest<IResult>;


    public class BorrowBookHandler : IRequestHandler<BorrowBookRequest, IResult>
    {
        private readonly BookListingDbContext _context;
        private readonly IAsyncPolicy _asyncPolicy;
        
        public BorrowBookHandler(BookListingDbContext context)
        {
            _context = context;
            
            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            _asyncPolicy = retryPolicy;


        }

        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
        
                app.MapPost("/books/{bookId}/borrow", async ([FromRoute] int bookId, [FromBody] int userId, IMediator mediator) =>
                {
                    var response = await mediator.Send(new BorrowBookRequest(bookId, userId));
                    return Results.Ok(response);
                }).WithTags("Books");
            }
        }

        public async Task<IResult> Handle(BorrowBookRequest request, CancellationToken cancellationToken)
        {
            return await _asyncPolicy.ExecuteAsync(async () =>
            {
                var book = await _context.Books.FindAsync(request.BookId, cancellationToken);
                if (book == null)
                {
                    return Results.NotFound();
                }

                if (book.ApiUserId != null)
                {
                    return Results.BadRequest();
                }
            
                book.ApiUserId = request.UserId;
                await _context.SaveChangesAsync(cancellationToken);
                return Results.Ok();
            });
        }
    }
}