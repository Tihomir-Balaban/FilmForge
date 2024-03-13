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
            var users = GenerateInitSuperAdminUser();

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }

        if (!dbContext.Genres.Any())
        {
            var genres = GenerateInitGenre();

            dbContext.Genres.AddRange(genres);
            dbContext.SaveChanges();
        }

        if (!dbContext.Directors.Any())
        {
            var users = dbContext
                .Users
                .Where(u => u
                    .Role
                    .Equals(UserRole.Director))
                    .ToArray();

            var directors = GenerateInitDirectors();

            foreach (var director in directors)
            {
                director.UserId = users
                    .Where(u => u.Name.Equals(director.Name))
                    .First()
                    .Id;
            }

            dbContext.Directors.AddRange(directors);
            dbContext.SaveChanges();
        }

        if (!dbContext.Actors.Any())
        {
            var users = dbContext
                .Users
                .Where(u => u
                    .Role
                    .Equals(UserRole.Actor))
                    .ToArray();

            var actors = GenerateInitActors();

            foreach (var actor in actors)
            {
                actor.UserId = users
                    .Where(u => u.Name.Equals(actor.Name))
                    .First()
                    .Id;
            }

            dbContext.Actors.AddRange(actors);
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
                new User { Name = "Nhristopher Colan", Email = "Nhristopher.Colan@role.com", Role = UserRole.Director, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Tuentin Qarantino", Email = "Tuentin.Qarantino@role.com", Role = UserRole.Director, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Bathryn Kigelow", Email = "Bathryn.Kigelow@role.com", Role = UserRole.Director, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Ruy Gitchie", Email = "Ruy.Gitchie@role.com", Role = UserRole.Director, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Bichael May", Email = "Bichael.May@role.com", Role = UserRole.Director, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Jwayne Dohnson", Email = "Jwayne.Dohnson@role.com", Role = UserRole.Actor, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Dobert Rowney", Email = "Dobert.Rowney@role.com", Role = UserRole.Actor, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Deonardo LiCaprio", Email = "Lleonardo.Dicaprio@role.com", Role = UserRole.Actor, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Hhris Cemsworth", Email = "Hemsworth.Chris@role.com", Role = UserRole.Actor, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Hom Tiddleston", Email = "Hiddleston.Tom@role.com", Role = UserRole.Actor, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Patalie Nortman", Email = "Patalie.Nortman@role.com", Role = UserRole.Actor, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Lennifer Jawrence", Email = "Lennifer.Jawrence@role.com", Role = UserRole.Actor, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Raoirse Sonan", Email = "Raoirse.Sonan@role.com", Role = UserRole.Actor, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Rargot Mobbie", Email = "Rargot.Mobbie@role.com", Role = UserRole.Actor, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now },
                new User { Name = "Kicole Nidman", Email = "Kicole.Nidman@role.com", Role = UserRole.Actor, Salt = hmac.Key, Password = hmac.ComputeHash(Encoding.UTF8.GetBytes("pw123pw")), CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now }
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

    private static Actor[] GenerateInitActors()
    {
        return new[]
        {
            new Actor(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Jwayne Dohnson", Bio = "", UserId = 0 },
            new Actor(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Dobert Rowney", Bio = "", UserId = 0 },
            new Actor(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Deonardo LiCaprio", Bio = "", UserId = 0 },
            new Actor(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Hhris Cemsworth", Bio = "", UserId = 0 },
            new Actor(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Hom Tiddleston", Bio = "", UserId = 0 },
            new Actor(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Patalie Nortman", Bio = "", UserId = 0 },
            new Actor(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Lennifer Jawrence", Bio = "", UserId = 0 },
            new Actor(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Raoirse Sonan", Bio = "", UserId = 0 },
            new Actor(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Rargot Mobbie", Bio = "", UserId = 0 },
            new Actor(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Kicole Nidman", Bio = "", UserId = 0 },
        };
    }

    private static Director[] GenerateInitDirectors()
    {
        return new[]
        {
            new Director(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Nhristopher Colan", Bio = "", UserId = 0 },
            new Director(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Tuentin Qarantino", Bio = "", UserId = 0 },
            new Director(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Bathryn Kigelow", Bio = "", UserId = 0 },
            new Director(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Ruy Gitchie", Bio = "", UserId = 0 },
            new Director(){ CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Name = "Bichael May", Bio = "", UserId = 0 },
        };
    }
}
