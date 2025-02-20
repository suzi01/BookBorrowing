using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Endpoint;
using WebApplication2.Models.Book;

namespace WebApplication2.Features.Books;

public class GetBook
{
    public record GetBookRequest(int BookId): IRequest<BookDTO>;


    public class GetBookHandler : IRequestHandler<GetBookRequest, BookDTO>
    {
        private readonly BookListingDbContext _context;
        private readonly IMapper _mapper;

        public GetBookHandler(BookListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public class Endpoint : IEndpoint
        {
            public void MapEndpoint(IEndpointRouteBuilder app)
            {

                app.MapGet("/books/{bookId}", async ([FromRoute] int bookId, IMediator mediator) =>
                {
                    var response = await mediator.Send(new GetBookRequest(bookId));
                    return Results.Ok(response);
                }).WithTags("Books");
            }
         }

        public async Task<BookDTO> Handle(GetBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(request.BookId, cancellationToken);
            if (book == null)
            {
                throw new Exception("Book not found");
            }
            var mappedBook = _mapper.Map<BookDTO>(book);
            return mappedBook;
        }
    }
}