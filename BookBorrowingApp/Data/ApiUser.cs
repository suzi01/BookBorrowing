using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Data;

public class ApiUser
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public virtual ICollection<Book> Books { get; set; }
}