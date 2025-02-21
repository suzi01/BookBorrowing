using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Data;

public class Book
{
    [Range(1, int.MaxValue)]
    [Key]
    public int BookId { get; set; }
    public required string Title { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public int? ApiUserId { get; set; }
    
    public ApiUser? ApiUser { get; set; }
}