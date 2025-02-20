using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication2.Data;
using WebApplication2.Features.Books;

namespace BookBorrowingApp.Tests.Features.BorrowBooks.Unit_Tests;

public class BorrowBooksHandlerTest
{
    
    [Fact]
    public async Task Handle_shouldReturnOKStatus_WhenBookFound()
    {
        var mockSet = new Mock<DbSet<Book>>();
        
        var mockContext = new Mock<BookListingDbContext>();
        mockContext.Setup(c => c.Books).Returns(mockSet.Object);

        var handler = new BorrowBook.BorrowBookHandler(mockContext.Object);
        
        var bookId = 1;
        var userId = 1;
        var expectedBook = new Book
            { BookId = 1, Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg", ApiUser = null};

        mockSet.Setup(m => m.FindAsync(bookId, CancellationToken.None)).ReturnsAsync(expectedBook);
        
        var borrowBookRequest = new BorrowBook.BorrowBookRequest(bookId, userId);
        
        var result = await handler.Handle(borrowBookRequest, CancellationToken.None);
        Assert.NotNull(result);
        Assert.IsAssignableFrom<Ok>(result);
        
    }
    
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenBookIsNotFound()
    {
        var mockSet = new Mock<DbSet<Book>>();
        
        var mockContext = new Mock<BookListingDbContext>();
        mockContext.Setup(c => c.Books).Returns(mockSet.Object);

        var handler = new BorrowBook.BorrowBookHandler(mockContext.Object);
        
        var bookId = 1;
        var userId = 1; 
        
        mockSet.Setup(m => m.FindAsync(bookId, CancellationToken.None)).ReturnsAsync(null as Book);
        
        var borrowBookRequest = new BorrowBook.BorrowBookRequest(bookId, userId);
        
        var result = await handler.Handle(borrowBookRequest, CancellationToken.None);
        Assert.NotNull(result);
        
        var notFoundResult = Assert.IsType<NotFound>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenApiUserIsNotNull()
    {
        var mockSet = new Mock<DbSet<Book>>();
        
        var mockContext = new Mock<BookListingDbContext>();
        mockContext.Setup(c => c.Books).Returns(mockSet.Object);
        

        var handler = new BorrowBook.BorrowBookHandler(mockContext.Object);
        
        var bookId = 1;
        var userId = 1;
        
        var expectedBook = new Book
            { BookId = 1, Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg", ApiUserId = userId};

        mockSet.Setup(m => m.FindAsync(bookId, CancellationToken.None)).ReturnsAsync(expectedBook);
        
        var borrowBookRequest = new BorrowBook.BorrowBookRequest(bookId, userId);
        
        var result = await handler.Handle(borrowBookRequest, CancellationToken.None);
        var badRequest = Assert.IsType<BadRequest>(result);
        Assert.Equal(400, badRequest.StatusCode);
    }
}