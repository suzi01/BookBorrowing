using System.Reflection;
using FluentValidation;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApplication2.Data;
using WebApplication2.Endpoint;
using WebApplication2.Features.Books;
using WebApplication2.Mapping;

namespace BookBorrowingApp.Tests;

internal class BookBorrowingWebApplicationFactory : WebApplicationFactory<Program>
{
    override protected void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseUrls("http://localhost:5035");
        builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<BookListingDbContext>));

                string? connectionString = GetConnectionString();
                services.AddSqlServer<BookListingDbContext>(connectionString);
                
                services.AddAutoMapper(typeof(MapperConfig));
                services.AddEndpoints(typeof(CreateBook.Endpoint).Assembly);
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
                services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                
                BookListingDbContext dbContext = CreateDbContext(services);
                dbContext.Database.EnsureDeleted(); 
                dbContext.Database.Migrate();
            }

        );
    }

    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.Test.json")
            .Build();
         var connString = configuration.GetConnectionString("BookListingDbTestingConnectionString");
         return connString;
    }

    private static BookListingDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookListingDbContext>();
        return dbContext;
    }
}