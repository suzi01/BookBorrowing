using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication2.Data.Configurations;

public class BookConfiguration:IEntityTypeConfiguration<Book>
{
    
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        {
            builder.HasData(
                new Book
                {
                    BookId = 1,
                    Title = "The Final Empire",
                    Author = "Brandon Sanderson",
                    Publisher = "Tor Books",
                    ApiUserId = 1
                },
                new Book
                {
                    BookId = 2,
                    Title = "The Well Of Ascension",
                    Author = "Brandon Sanderson",
                    Publisher = "Tor Books",
                    ApiUserId = 1
                },
                new Book
                {
                    BookId = 3,
                    Title = "Justice of Kings",
                    Author = "Richard Swan",
                    Publisher = "Tor Books",
                    ApiUserId = null
                },
                new Book
                {
                    BookId = 4,
                    Title = "Empire of Silence",
                    Author = "Christopher Rucchio",
                    Publisher = "Tor Books",
                    ApiUserId = null
                },
                new Book
                {
                    BookId = 5,
                    Title = "Tainted Cup",
                    Author = "Robert Jackson Bennet",
                    Publisher = "Tor Books",
                    ApiUserId = null
                },
                new Book
                {
                    BookId = 11,
                    Title = "Harry Potter and The Philosopher's stone ",
                    Author = "J.k.Rowling",
                    Publisher = "Bloomsbury",
                    ApiUserId = 2
                },
                new Book
                {
                    BookId = 12,
                    Title = "Harry Potter and The Chambers of Secrets ",
                    Author = "J.k.Rowling",
                    Publisher = "Bloomsbury",
                    ApiUserId = 2
                },
                new Book
                {
                    BookId = 13,
                    Title = "Harry Potter and The Prisoner of Azkaban ",
                    Author = "J.k.Rowling",
                    Publisher = "Bloomsbury",
                    ApiUserId = 2
                },
                new Book
                {
                    BookId = 14,
                    Title = "Harry Potter and The Goblet of Fire ",
                    Author = "J.k.Rowling",
                    Publisher = "Bloomsbury",
                    ApiUserId = 3
                });
        }
    }
}