using FilmForge.Common.Enum;
using FilmForge.Entities.Context;
using FilmForge.Entities.EntityModels;
using System.Security.Cryptography;
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
        using (var hmac = new HMACSHA512())
        {
            return new User
            {
                Name = "Tihomir Balaban",
                Email = "tihomir@balaban.com",
                Role = UserRole.SuperAdministrator,
                Salt = hmac.Key,
                Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")),
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
        }
    }
}
