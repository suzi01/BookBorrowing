using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Endpoint;
using WebApplication2.Models.Book;

namespace WebApplication2.Features.Books;

public class GetAllBooks
{
    public record GetAllBooksRequest():IRequest<IEnumerable<BookDTO>>;


    public class GetAllBooksHandler : IRequestHandler<GetAllBooksRequest, IEnumerable<BookDTO>>
    {
        private readonly BookListingDbContext _context;
        private readonly IMapper _mapper;

        public GetAllBooksHandler(BookListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public class Endpoint : IEndpoint
        {
            
            public void MapEndpoint(IEndpointRouteBuilder app)
            {
                app.MapGet("/books", async ([FromServices] IMediator mediator) =>
                {
                    var response = await mediator.Send(new GetAllBooksRequest());
                    return Results.Ok(response);
                }).WithTags("Books");
            }
        }

        public async Task<IEnumerable<BookDTO>> Handle(GetAllBooksRequest request, CancellationToken cancellationToken)
        {
            var books = await _context.Books.ToListAsync(cancellationToken);
            var mappedBooks = _mapper.Map<IEnumerable<BookDTO>>(books);
            return mappedBooks;
        }
    }
}