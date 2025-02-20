using WebApplication2.Models.Book;

namespace WebApplication2.Models.User;

public class UserDto: BaseUserDto
{
    public int Id { get; set; }
    public ICollection<BookDTO> Books { get; set; }
}