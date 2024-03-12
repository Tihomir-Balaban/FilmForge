using FilmForge.Common.Enum;
using FilmForge.Entities.Context;
using FilmForge.Entities.EntityModels;
using System.Text;

namespace FilmForge.Entities.Initializer;

public static class DbInitializer
{
    public static void Initialize(FilmForgeDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        if (!dbContext.Users.Any())
        {
            var user = GenerateInitSuperAdminUser();

            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }
        else
            return;
    }

    private static User GenerateInitSuperAdminUser()
    {
        var str = "pw123pw";

        return new User
        {
            Name = "Tihomir Balaban",
            Email = "tihomir@balaban.com",
            Password = Encoding.ASCII.GetBytes(str),
            Role = UserRole.SuperAdministrator
        };
    }
}
