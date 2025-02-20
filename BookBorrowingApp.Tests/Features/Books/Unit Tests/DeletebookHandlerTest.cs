using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication2.Data;
using WebApplication2.Features.Books;

namespace BookBorrowingApp.Tests.Features.Books.Unit_Tests;

public class DeleteHandlerbookTest
{

    private readonly Mock<BookListingDbContext> _mockDbContext;

    public DeleteHandlerbookTest()
    {
        _mockDbContext = new Mock<BookListingDbContext>();
    }

    [Fact]
    public async Task DeleteHandler_Handle_GivenValidId_ShouldReturnNoContent()
    {
        
        var mockSet = new Mock<DbSet<Book>>();
        
        _mockDbContext.Setup(x => x.Books).Returns(mockSet.Object);

        var expectedDto = new Book
            { BookId = 1, Author = "George Orwell", Title = "1984", Publisher = "Secker & Warburg" };
        
        var validId = 1;
        var validRequest = new DeleteBook.DeleteBookRequest(validId);
        
        mockSet.Setup(m => m.FindAsync(validId, CancellationToken.None)).ReturnsAsync(expectedDto);
        
        var handler = new DeleteBook.DeleteBookHandler(_mockDbContext.Object);
        
        var result = await handler.Handle(validRequest, CancellationToken.None);

        var noContentResult = Assert.IsType<NoContent>(result);
        Assert.Equal(204, noContentResult.StatusCode);
    }

    [Fact]
    public async Task Handle_GivenInvalidId_ShouldReturnNotFound()
    {
        var mockSet = new Mock<DbSet<Book>>();
        
        _mockDbContext.Setup(x => x.Books).Returns(mockSet.Object);
        
        var invalidId = 1;
        var invalidRequest = new DeleteBook.DeleteBookRequest(invalidId);
        
        mockSet.Setup(m => m.FindAsync(invalidId, CancellationToken.None));
        
        var handler = new DeleteBook.DeleteBookHandler(_mockDbContext.Object);
        
        var result = await handler.Handle(invalidRequest, CancellationToken.None);

        var notFoundResult = Assert.IsType<NotFound>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
    
}