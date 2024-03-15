using FilmForge.Common.Enum;
using FilmForge.Entities.Context;
using FilmForge.Models.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace FilmForge.Repository.Intergration.Tests.Base;

public class BaseRepositoryIntergrationsTest<T> : IDisposable
{
    protected readonly FilmForgeDbContext filmForgeDbContext;
    private protected readonly IMapper mapper;
    private protected readonly Mock<ILogger<T>> loggerMock = new();

    public BaseRepositoryIntergrationsTest()
    {
        var options = new DbContextOptionsBuilder<FilmForgeDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        filmForgeDbContext = new FilmForgeDbContext(options);

        filmForgeDbContext.Database.EnsureCreated();

        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutoMappingProfiles());
        });

        mapper = mappingConfig.CreateMapper();
    }

    public void Dispose()
    {
        filmForgeDbContext.Database.EnsureDeleted();
        filmForgeDbContext.Dispose();
    }

    protected User[] GenerateUsers()
    {
        filmForgeDbContext.Users.RemoveRange(filmForgeDbContext.Users);

        using (var hmac = new HMACSHA512())
        {
            var users = new Faker<User>()
                .RuleFor(x => x.CreatedOn, y => DateTime.Now)
                .RuleFor(x => x.ModifiedOn, y => DateTime.Now)
                .RuleFor(x => x.Name, y => y.Person.FullName)
                .RuleFor(x => x.Email, y => y.Person.Email)
                .RuleFor(
                    x => x.Password, y => hmac
                    .ComputeHash(
                        Encoding
                        .UTF8
                        .GetBytes(y.Internet.Password())))
                .RuleFor(x => x.Salt, y => hmac.Key)
                .RuleFor(x => x.Role, y => y.PickRandom<UserRole>())
                .Generate(10)
                .ToArray();

            filmForgeDbContext.Users.AddRange(users);
            filmForgeDbContext.SaveChanges();

            return users;
        }
    }

    protected Actor[] GenerateActors(User[] users)
    {
        filmForgeDbContext.Actors.RemoveRange(filmForgeDbContext.Actors);
        var actors = new List<Actor>();

        foreach (var user in users)
        {
            var actor = new Faker<Actor>()
                .RuleFor(x => x.CreatedOn, y => DateTime.Now)
                .RuleFor(x => x.ModifiedOn, y => DateTime.Now)
                .RuleFor(x => x.Name, y => y.Person.FullName)
                .RuleFor(x => x.Bio, y => y.Lorem.Sentence(20))
                .RuleFor(x => x.Fee, y => y.Random.ULong())
                .RuleFor(x => x.UserId, y => user.Id)
                .Generate();

            actors.Add(actor);
        }

        filmForgeDbContext.Actors.AddRange(actors.ToArray());
        filmForgeDbContext.SaveChanges();

        return actors.ToArray();
    }
}
