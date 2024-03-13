using FilmForge.Entities.Context;
using FilmForge.Models.Profiles;
using Microsoft.EntityFrameworkCore;

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
}
