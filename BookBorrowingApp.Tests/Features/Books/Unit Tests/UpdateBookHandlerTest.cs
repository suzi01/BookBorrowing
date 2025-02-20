using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication2.Data;
using WebApplication2.Features.Books;
using WebApplication2.Models.Book;

namespace BookBorrowingApp.Tests.Features.Books.Unit_Tests;


public class UpdateBookHandlerTest
{
    
    [Fact]
    public async Task Handle_GivenValidRequest_ShouldUpdateBook()
    {
        
        var mockDbSet = new Mock<DbSet<Book>>();
        
        var mockContext = new Mock<BookListingDbContext>();
        mockContext.Setup(x => x.Books).Returns(mockDbSet.Object);

        var mockMapper = new Mock<IMapper>();

        var bookId = 1;
        
        var updatedBookDto = new BookDTO {  Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Publisher = "Charles Scribner's Sons" };
        var updatedBook = new Book
            { BookId = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Publisher = "Charles Scribner's Sons" };
        
        mockContext.Setup(c => c.Books.FindAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedBook); 
        
        mockMapper.Setup(m => m.Map<Book>(updatedBookDto)).Returns(updatedBook);
        mockMapper.Setup(m => m.Map<BookDTO>(updatedBook)).Returns(updatedBookDto);
   
        var validUpdate = new UpdateBook.UpdateBookRequest(bookId, updatedBookDto);
        var handler = new UpdateBook.UpdateBookHandler(mockContext.Object, mockMapper.Object);
        
        var result = await handler.Handle(validUpdate, CancellationToken.None);
        
        Assert.NotNull(result);
        
        Assert.IsType<Ok<BookDTO>>(result);
        var okResult = (Ok<BookDTO>) result;
        
        Assert.NotNull(okResult.Value);
        Assert.Equivalent(updatedBookDto, okResult.Value);
        
    }
    
    [Fact]
    public async Task Handle_GivenInvalidRequest_ShouldReturnNotFound()
    {
        
        var mockDbSet = new Mock<DbSet<Book>>();
        
        var mockContext = new Mock<BookListingDbContext>();
        mockContext.Setup(x => x.Books).Returns(mockDbSet.Object);
        
        var mockMapper = new Mock<IMapper>();
        
        var invalidBookId = 1;
        var updatedBookDto = new BookDTO {  Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Publisher = "Charles Scribner's Sons" };
        
        var handler = new UpdateBook.UpdateBookHandler(mockContext.Object, mockMapper.Object);
        var validUpdate = new UpdateBook.UpdateBookRequest(invalidBookId, updatedBookDto);
        
        var result = await handler.Handle(validUpdate, CancellationToken.None);
        
        Assert.NotNull(result);
        
        var notFoundResult = Assert.IsType<NotFound>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
}