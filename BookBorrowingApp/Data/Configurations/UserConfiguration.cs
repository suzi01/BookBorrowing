using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication2.Data.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<ApiUser>
{
    
    public void Configure(EntityTypeBuilder<ApiUser> builder)
    {
  
            builder.HasData(
                new ApiUser 
                {
                    Id = 1, FirstName = "Jenny", LastName = "Crews", Email = "JennyCrews@hotmail.com", Password = "JennyPassword12!"
                }, 
                new ApiUser
                {
                    Id = 2, FirstName = "Harry", LastName = "Styles", Email = "watermelonSugar@hotmail.com", Password = "WatermelonPassword3e?"
                }, 
                new ApiUser
                {
                    Id = 3, FirstName = "Jin", LastName = "Sakai", Email = "tshimaGhost@hotmail.com", Password = "JinPassword!23"
                });
    }
}