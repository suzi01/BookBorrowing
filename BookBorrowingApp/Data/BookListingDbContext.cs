using Microsoft.EntityFrameworkCore;
using WebApplication2.Data.Configurations;

namespace WebApplication2.Data;

public class BookListingDbContext : DbContext
{
    
    public BookListingDbContext(DbContextOptions<BookListingDbContext> options) : base(options)
    {
      
    }
    
    public BookListingDbContext() { }

    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<ApiUser> ApiUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>()
            .HasOne(b => b.ApiUser)
            .WithMany(u => u.Books)
            .HasForeignKey(b => b.ApiUserId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        
    }
    
}