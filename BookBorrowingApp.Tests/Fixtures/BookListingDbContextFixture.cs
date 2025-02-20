using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication2.Data;
using WebApplication2.Features.Books;
using WebApplication2.Models.Book;

namespace BookBorrowingApp.Tests.Fixtures;

public class BookListingDbContextFixture: IDisposable
{
    public  BookListingDbContext Context { get; private set; }

    public BookListingDbContextFixture()
    {
        var options = new DbContextOptionsBuilder<BookListingDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;
        
        
        
        Context = new BookListingDbContext(options);
        
        SeedDatabase();
        
        // var configuration = new ConfigurationBuilder()
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //     .AddJsonFile("appsettings.Test.json")
        //     .Build();
        //
        // var connectionString = configuration.GetConnectionString("BookListingDbTestingConnectionString");
        //
        // var options = new DbContextOptionsBuilder<BookListingDbContext>()
        //     .UseSqlServer(connectionString, sqlOptions =>
        //     {
        //         sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        //     })
        //     .Options;
        //
        // Context = new BookListingDbContext(options, configuration);
        //
        // // Apply Migrations
        // // Context.Database.EnsureDeleted(); 
        // Context.Database.Migrate();
        //
        // // Seed Data
        // SeedDatabase();
    }
    
    public void ResetDatabase()
    {
        // Context.ChangeTracker.Clear();
        Context.Books.RemoveRange(Context.Books);
        Context.SaveChanges();
    }

    private void SeedDatabase()
    {
        Context.Books.AddRange(
            new Book
            {
                Title = "Test Book 1",
                Author = "Author 1",
                Publisher = "Publisher 1"
            },
            new Book
            {
                Title = "Test Book 2",
                Author = "Author 2",
                Publisher = "Publisher 2"
            }
        );
        Context.SaveChanges();
        
    }

    public void ClearDatabase()
    {
        Context.Books.RemoveRange(Context.Books);
        Context.SaveChanges();
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    
    
    public void Dispose()
    {
        // ClearDatabase();
        Context.Database.EnsureDeleted();  
        Context.Dispose();
    }
}