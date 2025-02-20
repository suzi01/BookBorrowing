using AutoMapper;
using WebApplication2.Data;
using WebApplication2.Features.Books;
using WebApplication2.Features.Users;
using WebApplication2.Models.Book;
using WebApplication2.Models.User;

namespace WebApplication2.Mapping;

public class MapperConfig:Profile
{
    public MapperConfig()
    {
        CreateMap<Book, BookDTO>().ReverseMap();
        CreateMap<CreateBook.CreateBookRequest, Book>().ReverseMap();
        CreateMap<CreateBook.CreateBookRequest, BookDTO>().ReverseMap();
        CreateMap<ApiUser, UserDto>().ReverseMap();
        CreateMap<ApiUser, CreateUserDto>().ReverseMap();
        CreateMap<CreateUser.CreateUserRequest, ApiUser>().ReverseMap();
        CreateMap<UpdateUserDto, ApiUser>().ReverseMap();
    }
}