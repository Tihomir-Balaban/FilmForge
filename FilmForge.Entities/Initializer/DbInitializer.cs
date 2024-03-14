using FilmForge.Common.Enum;
using FilmForge.Entities.Context;
using FilmForge.Entities.EntityModels;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FilmForge.Entities.Initializer;

public static class DbInitializer
{
    private static Actor[] Actors;
    private static Director[] Directors;
    private static Genre[] Genres;

    public static void Initialize(FilmForgeDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        Actors = GenerateInitActors();
        Directors = GenerateInitDirectors();
        Genres = GenerateInitGenre();


        if (!dbContext.Users.Any())
        {
            var users = GenerateInitSuperAdminUser();

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
        }

        if (!dbContext.Genres.Any())
        {
            dbContext.Genres.AddRange(Genres);
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

            foreach (var director in Directors)
            {
                director.UserId = users
                    .Where(u => u.Name.Equals(director.Name))
                    .First()
                    .Id;
            }

            dbContext.Directors.AddRange(Directors);
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

            foreach (var actor in Actors)
            {
                actor.UserId = users
                    .Where(u => u.Name.Equals(actor.Name))
                    .First()
                    .Id;
            }

            dbContext.Actors.AddRange(Actors);
            dbContext.SaveChanges();
        }
        if (!dbContext.Movies.Any())
        {
            dbContext.Movies.AddRange(GenerateInitMovies());
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

    private static Movie[] GenerateInitMovies()
    {
        return new[]
        {
            new Movie() { Title = "Shadows of the Forgotten", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[0].Id, DirectorId = Directors[0].Id, Actors = new List<Actor>(){ Actors[0], Actors[1] }},
            new Movie() { Title = "Echoes in the Mist", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[1].Id, DirectorId = Directors[0].Id, Actors = new List<Actor>(){ Actors[0], Actors[1] }},
            new Movie() { Title = "Whispers of Destiny", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[2].Id, DirectorId = Directors[0].Id, Actors = new List<Actor>(){ Actors[0], Actors[1] }},
            new Movie() { Title = "The Last Horizon", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[3].Id, DirectorId = Directors[0].Id, Actors = new List<Actor>(){ Actors[0], Actors[1] }},
            new Movie() { Title = "Beyond the Void", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[4].Id, DirectorId = Directors[1].Id, Actors = new List<Actor>(){ Actors[2], Actors[3] }},
            new Movie() { Title = "Crimson Twilight", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[5].Id, DirectorId = Directors[1].Id, Actors = new List<Actor>(){ Actors[2], Actors[3] }},
            new Movie() { Title = "Echoes of Tomorrow", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[6].Id, DirectorId = Directors[1].Id, Actors = new List<Actor>(){ Actors[2], Actors[3] }},
            new Movie() { Title = "The Veil of Secrets", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[7].Id, DirectorId = Directors[1].Id, Actors = new List<Actor>(){ Actors[2], Actors[3] }},
            new Movie() { Title = "A Whisper of Truth", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[8].Id, DirectorId = Directors[2].Id, Actors = new List<Actor>(){ Actors[4], Actors[5] }},
            new Movie() { Title = "The Eternal Dance", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[9].Id, DirectorId = Directors[2].Id, Actors = new List<Actor>(){ Actors[4], Actors[5] }},
            new Movie() { Title = "Lost in the Echo", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[10].Id, DirectorId = Directors[2].Id, Actors = new List<Actor>(){ Actors[4], Actors[5] }},
            new Movie() { Title = "Veins of the Earth", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[11].Id, DirectorId = Directors[2].Id, Actors = new List<Actor>(){ Actors[4], Actors[5] }},
            new Movie() { Title = "The Silent Awakening", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[12].Id, DirectorId = Directors[3].Id, Actors = new List<Actor>(){ Actors[6], Actors[7] }},
            new Movie() { Title = "Beneath the Starlit Sky", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[13].Id, DirectorId = Directors[3].Id, Actors = new List<Actor>(){ Actors[6], Actors[7] }},
            new Movie() { Title = "The Unseen Path", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[14].Id, DirectorId = Directors[3].Id, Actors = new List<Actor>(){ Actors[6], Actors[7] }},
            new Movie() { Title = "Whirlwind of Fate", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[15].Id, DirectorId = Directors[3].Id, Actors = new List<Actor>(){ Actors[6], Actors[7] }},
            new Movie() { Title = "The Final Reckoning", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[16].Id, DirectorId = Directors[4].Id, Actors = new List<Actor>(){ Actors[8], Actors[9] }},
            new Movie() { Title = "Flames of Redemption", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[17].Id, DirectorId = Directors[4].Id, Actors = new List<Actor>(){ Actors[8], Actors[9] }},
            new Movie() { Title = "The Shadows' Embrace", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[18].Id, DirectorId = Directors[4].Id, Actors = new List<Actor>(){ Actors[8], Actors[9] }},
            new Movie() { Title = "Echoes of the Abyss", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, Budget = 5678165, GenreId = Genres[19].Id, DirectorId = Directors[4].Id, Actors = new List<Actor>(){ Actors[8], Actors[9] }}
        };
    }
}
