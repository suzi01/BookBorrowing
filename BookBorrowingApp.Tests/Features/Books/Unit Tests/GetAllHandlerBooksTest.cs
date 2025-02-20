using AutoMapper;
using MockQueryable.Moq;
using Moq;
using WebApplication2.Data;
using WebApplication2.Features.Books;
using WebApplication2.Models.Book;

namespace BookBorrowingApp.Tests.Features.Books.Unit_Tests;


public class GetAllBooksHandlerTest
{

    [Fact]
    public async Task ShouldReturnEmptyListIfNoBooksFound()
    {
        var books = new List<Book>
        {
            new Book { BookId = 1, Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" },
            new Book
            {
                BookId = 2, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald",
                Publisher = "Charles Scribner's Sons"
            },
            new Book { BookId = 3, Title = "Moby-Dick", Author = "Herman Melville", Publisher = "Harper & Brothers" },
        };

        var bookDtos = new List<BookDTO>
        {
            new BookDTO { Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" },
            new BookDTO
            {
                Title = "The Great Gatsby", Author = "F. Scott Fitzgerald",
                Publisher = "Charles Scribner's Sons"
            },
            new BookDTO { Title = "Moby-Dick", Author = "Herman Melville", Publisher = "Harper & Brothers" },
        };
        //
        var mockSet = books.AsQueryable().BuildMockDbSet();
        
        var mockContext = new Mock<BookListingDbContext>();
        mockContext.Setup(m => m.Books).Returns(mockSet.Object);
     
        
        
        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<IEnumerable<BookDTO>>(books)).Returns(bookDtos);
        var handler = new GetAllBooks.GetAllBooksHandler(mockContext.Object, mockMapper.Object);
        
        // Act
        var query = new GetAllBooks.GetAllBooksRequest(); ;
               
        var result = await handler.Handle(query, CancellationToken.None);
      

        //Assert
        Assert.NotNull(result);

        Assert.Equal(3, result.ToList().Count);
        Assert.Equal(bookDtos, result.ToList());
        
    }
    
    
}