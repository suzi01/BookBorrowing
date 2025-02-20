using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebApplication2.Data;
using WebApplication2.Features.Books;

namespace BookBorrowingApp.Tests.Features.Books.Integration_Test;

public class BookIntegrationTests: IClassFixture<BookBorrowingWebApplicationFactory>
{
    private record ValidationError(string PropertyName, string ErrorMessage); 


    [Fact]
    public async Task CreateBook_WithValidData_ReturnsSuccessfully()
    {
        var application = new BookBorrowingWebApplicationFactory();
        var createBookRequest = new CreateBook.CreateBookRequest("New book", "New Author",  "New Publisher");
        var requestUrl = "/books";
        
        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync(requestUrl, createBookRequest);
        
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var book = await response.Content.ReadFromJsonAsync<Book>();
        book.Should().NotBeNull();
        book.Should().BeEquivalentTo(createBookRequest);
    }
    
    [Theory]
    [InlineData("", "New Author",  "New Publisher", "Title is required in order to create book")]
    [InlineData("New Author", "",  "New Publisher", "Author is required in order to create book")]
    [InlineData("New Author", "New Author",  "", "Publisher is required in order to create book")]
    public async Task CreateBook_WithInvalidData_ReturnsBadRequest(string title, string author, string publisher, string errorMessage)
    {
        var application = new BookBorrowingWebApplicationFactory();
        var createBookRequest = new CreateBook.CreateBookRequest(title, author,  publisher);
        var requestUrl = "/books";
        
        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync(requestUrl, createBookRequest);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    
        var errors = await response.Content.ReadFromJsonAsync<List<ValidationError>>();
        errors.Should().NotBeNull();
        errors[0].ErrorMessage.Should().Be(errorMessage);
    }
    
    
    [Fact]
    public async Task GetAllBooks_ReturnsSuccessfully()
    {
        var application = new BookBorrowingWebApplicationFactory();
     
        var requestUrl = "/books";
        
        var client = application.CreateClient();
      
        var response = await client.GetAsync(requestUrl);
        
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetBook_WithValidId_ReturnsSuccessfully()
    {
        var application = new BookBorrowingWebApplicationFactory();
        var requestUrl = "/books/1";
        
        var client = application.CreateClient();
        var response = await client.GetAsync(requestUrl);
        
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var book = await response.Content.ReadFromJsonAsync<Book>();
        book.Should().NotBeNull();
        book.BookId.Should().Be(1);
    }
    
    [Fact]
    public async Task GetBook_WithInvalidId_ThrowsException()
    {
        var application = new BookBorrowingWebApplicationFactory();
        var requestUrl = "/books/6";
        
        var client = application.CreateClient();
        var response = await client.GetAsync(requestUrl);
        
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}

