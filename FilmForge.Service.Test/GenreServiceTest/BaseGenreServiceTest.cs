using FilmForge.Repository.GenreRepository;
using GS = FilmForge.Service.GenreService;

namespace FilmForge.Service.Test.GenreServiceTest;

public class BaseGenreServiceTest
{
    protected readonly Mock<IGenreRepository> GenreRepositoryMock = new();
    protected readonly Mock<ILogger<GS.GenreService>> LoggerMock = new();
    protected readonly Mock<IMapper> MapperMock = new();

    protected GS.GenreService GenreService;

    private protected void InitService()
    {
        GenreService = new(
            GenreRepositoryMock.Object,
            LoggerMock.Object,
            MapperMock.Object);
    }

    private protected GenreDto[] ArrangeGenreDtos(int amount)
    {
        return new Faker<GenreDto>()
            .RuleFor(x => x.Name, y => y.Person.FullName)
            .RuleFor(x => x.Movies, y => new Faker<MovieDto>().Generate(3))
            .Generate(amount)
            .ToArray();
    }

    private protected Genre[] ArrangeGenreEntities(int amount)
    {
        return new Faker<Genre>()
            .RuleFor(x => x.Name, y => y.Person.FullName)
            .RuleFor(x => x.Movies, y => new Faker<Movie>().Generate(3))
            .Generate(amount)
            .ToArray();
    }
}
