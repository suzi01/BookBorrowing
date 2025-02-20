using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApplication2.Data;
using WebApplication2.Features.Books;
using WebApplication2.Models.Book;

namespace BookBorrowingApp.Tests.Features.Books.Unit_Tests;


public class CreateBookHandlerTest
{
    [Fact]
    public async Task CreateBook_Should_Create_A_Book_With_Book_Details()
    {
        var mockDbSet = new Mock<DbSet<Book>>();
        var mockContext = new Mock<BookListingDbContext>();
        var mockMapper = new Mock<IMapper>();
        
        var newBookRequest = new CreateBook.CreateBookRequest("New book", "New Author",  "New Publisher");
        
        var bookDto = new BookDTO
        {
            Author = newBookRequest.Author,
            Publisher = newBookRequest.Publisher,
            Title = newBookRequest.Title,
        };
        
        mockMapper.Setup(m => m.Map<BookDTO>(newBookRequest)).Returns(bookDto);
        
        var bookEntity = new Book {BookId = 1, Title="New book", Author="New Author", Publisher="New Publisher"};
        mockMapper.Setup(m => m.Map<Book>(bookDto)).Returns(bookEntity);

        
        mockContext.Setup(x => x.Books).Returns(mockDbSet.Object);
        mockDbSet.Setup(m => m.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()));
        
        var handler = new CreateBook.CreateBookHandler(mockContext.Object, mockMapper.Object);
        
        var result = await handler.Handle(newBookRequest , CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(bookEntity.Author, result.Author);
        Assert.Equal(bookEntity.Title, result.Title);
        Assert.Equal(bookEntity.Publisher, result.Publisher);

    }
    
    
}