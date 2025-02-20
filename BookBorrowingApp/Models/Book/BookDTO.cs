using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.Book;

public class BookDTO
{
    [Required]
    public string Title { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }   
    
    public int BookId { get; set; }
}