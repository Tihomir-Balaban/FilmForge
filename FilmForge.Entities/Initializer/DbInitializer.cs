using FilmForge.Common.Enum;
using FilmForge.Entities.Context;
using FilmForge.Entities.EntityModels;
using System;
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
            var users = GenerateInitSuperAdminUser();

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }
        else if (!dbContext.Genres.Any())
        {
            var genres = GenerateInitGenre();

            dbContext.Genres.AddRange(genres);
            dbContext.SaveChanges();
        }
        else
            return;
    }

    private static User[] GenerateInitSuperAdminUser()
    {
        using (var hmac = new HMACSHA512())
        {
            return new[]
            {
                new User
                {
                    Name = "Tihomir Balaban",
                    Email = "tihomir@balaban.com",
                    Role = UserRole.SuperAdministrator,
                    Salt = hmac.Key,
                    Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },
                new User
                {
                    Name = "Director Directorson",
                    Email = "director@role.com",
                    Role = UserRole.Director,
                    Salt = hmac.Key,
                    Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },
                new User
                {
                    Name = "Actor Actorson",
                    Email = "actor@role.com",
                    Role = UserRole.Actor,
                    Salt = hmac.Key,
                    Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                }
            };             
        }
    }


    private static Genre[] GenerateInitGenre()
    {
        return new[]
        {
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Action" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Adventure" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Comedy" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Drama" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Fantasy" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Historical" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Horror" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Science Fiction (Sci-Fi)" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Musical/Dance" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Mystery" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Romance" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Thriller/Suspense" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Western" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Documentary" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Biographical (Biopic)" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Animation" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Family" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "War" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Crime" },
            new Genre(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Film Noir" },
        };
    }
}
