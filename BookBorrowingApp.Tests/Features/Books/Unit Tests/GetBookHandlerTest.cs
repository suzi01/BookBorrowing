using AutoMapper;
using BookBorrowingApp.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication2.Data;
using WebApplication2.Features.Books;
using WebApplication2.Models.Book;


namespace BookBorrowingApp.Tests.Features.Books.Unit_Tests;


public class GetBookHandlerTest
{

    [Fact]
    public async Task Handle_ShouldThrowException_WhenBookIsNotFound()
    {
        var mockSet = new Mock<DbSet<Book>>();
        
        var mockContext = new Mock<BookListingDbContext>();
        mockContext.Setup(c => c.Books).Returns(mockSet.Object);
        
        var mockMapper = new Mock<IMapper>();

        var handler = new GetBook.GetBookHandler(mockContext.Object, mockMapper.Object);
        
        var invalidBookId = 3;
        
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(new GetBook.GetBookRequest(invalidBookId), CancellationToken.None));
        
        Assert.Equal("Book not found", exception.Message);
    }
    
    
    [Fact]
    public async Task Handle_ShouldReturnBook_WhenBookExists()
    {

        var mockSet = new Mock<DbSet<Book>>();
        
        var mockContext = new Mock<BookListingDbContext>();
        mockContext.Setup(c => c.Books).Returns(mockSet.Object);


        var mockMapper = new Mock<IMapper>();

        var handler = new GetBook.GetBookHandler(mockContext.Object, mockMapper.Object);


        var bookId = 1;
        var expectedBook = new Book
            { BookId = 1, Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" };
        var expectedBookDto = new BookDTO { Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" };


        mockSet.Setup(m => m.FindAsync(bookId, CancellationToken.None)).ReturnsAsync(expectedBook);
        mockMapper.Setup(m => m.Map<BookDTO>(expectedBook)).Returns(expectedBookDto);


        var request = new GetBook.GetBookRequest(bookId);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equivalent(expectedBookDto, result); 
    }
    
    // [Fact]
    // public async Task GetTodoReturnsNotFoundIfNotExists()
    // {
    //     // Arrange
    //     
    //     var data = new List<Book>
    //     {
    //         new Book { BookId = 1, Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" },
    //         new Book { BookId = 2, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Publisher = "Charles Scribner's Sons"},
    //         new Book { BookId = 3, Title = "Moby-Dick", Author = "Herman Melville", Publisher = "Harper & Brothers"},
    //     }.AsQueryable();
    //     //
    //     var mockSet = new Mock<DbSet<Book>>();
    //     mockSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(data.Provider);
    //     mockSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(data.Expression);
    //     mockSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(data.ElementType);
    //     mockSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
    //     
    //     var mockContext = new Mock<BookListingDbContext>();
    //     mockContext.Setup(m => m.Books).Returns(mockSet.Object);
    //     
    //     var mockMapper = new Mock<IMapper>();
    //     
    //     mockMapper.Setup(m => m.Map<BookDTO>(It.Is<Book>(b => b.BookId == 1))).Returns(new BookDTO { Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" });
    //     
    //     var getBookHandler = new GetBook.GetBookHandler(_context, mockMapper.Object);
    //     var bookId = 1;
    //     var request = new GetBook.GetBookRequest(bookId);
    //     var result = await getBookHandler.Handle(request, It.IsAny<CancellationToken>());
    //     var expectedBookDto = new BookDTO { Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" };
    //     
    //     Assert.NotNull(result);
    //     Assert.Equal(expectedBookDto, result);
    // }




    // [Fact]
    // public async Task Handle_ShouldReturnBook_WhenBookIsFound()
    // {

    // var data = new List<Book>
    // {
    //     new Book { BookId = 1, Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" },
    //     new Book { BookId = 2, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Publisher = "Charles Scribner's Sons"},
    //     new Book { BookId = 3, Title = "Moby-Dick", Author = "Herman Melville", Publisher = "Harper & Brothers"},
    // }.AsQueryable();
    // //
    // var mockSet = new Mock<DbSet<Book>>();
    // mockSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(data.Provider);
    // mockSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(data.Expression);
    // mockSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(data.ElementType);
    // mockSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

    // var mockContext = new Mock<BookListingDbContext>();
    // mockContext.Setup(m => m.Books).Returns(mockSet.Object);


    // var mockMapper = new Mock<IMapper>();
    //
    // mockMapper.Setup(m => m.Map<BookDTO>(It.Is<Book>(b => b.BookId == 1))).Returns(new BookDTO { Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" });
    //
    // var getBookHandler = new GetBook.GetBookHandler(_context, mockMapper.Object);
    //
    // var bookId = 1;
    // var request = new GetBook.GetBookRequest(bookId);
    // var result = await getBookHandler.Handle(request, It.IsAny<CancellationToken>());
    // var expectedBookDto = new BookDTO { Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" };
    //
    // Assert.Equal(expectedBookDto, result);
    // result.Should().BeEquivalentTo(expectedBookDt);

    // }

    // [Fact]
    // public async Task Handle_ShouldReturnBook_WhenBookIsFound()
    // {
    //     var data = new List<Book>
    //     {
    //         new Book { BookId = 1, Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" },
    //         new Book { BookId = 2, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Publisher = "Charles Scribner's Sons"},
    //         new Book { BookId = 3, Title = "Moby-Dick", Author = "Herman Melville", Publisher = "Harper & Brothers"},
    //     }.AsQueryable();
    //     
    //     var mockSet = new Mock<DbSet<Book>>();
    //     mockSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(data.Provider);
    //     mockSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(data.Expression);
    //     mockSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(data.ElementType);
    //     mockSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
    //
    //     var mockContext = new Mock<BookListingDbContext>();
    //     mockContext.Setup(m => m.Books).Returns(mockSet.Object);
    //
    //     var mockMapper = new Mock<IMapper>();
    //
    //     // *** Setup mapping for the *specific* book you expect ***
    //     mockMapper.Setup(m => m.Map<BookDTO>(It.Is<Book>(b => b.BookId == 1)))
    //         .Returns(new BookDTO { Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" });
    //
    //     // Setup mapping for a different book to demonstrate failure if incorrect book is returned.
    //     // mockMapper.Setup(m => m.Map<BookDTO>(It.Is<Book>(b => b.BookId == 2)))
    //     //    .Returns(new BookDTO { BookId = 2, Author = "F. Scott Fitzgerald", Title = "The Great Gatsby", Publisher = "Charles Scribner's Sons" });
    //
    //
    //     var getBookHandler = new GetBook.GetBookHandler(mockContext.Object, mockMapper.Object);
    //     var bookId = 1;
    //     var request = new GetBook.GetBookRequest(bookId);
    //
    //     var result = await getBookHandler.Handle(request, CancellationToken.None);
    //     
    //     var expectedBookDto = new BookDTO { Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" };
    //     Assert.Equivalent(expectedBookDto, result); 
    // }
    
    
    

}

    