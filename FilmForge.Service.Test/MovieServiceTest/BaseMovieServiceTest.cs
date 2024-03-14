using FilmForge.Repository.MovieRepository;
using MS = FilmForge.Service.MovieService;

namespace FilmForge.Service.Test.MovieServiceTest;

public class BaseMovieServiceTest
{
    protected readonly Mock<IMovieRepository> MovieRepositoryMock = new();
    protected readonly Mock<ILogger<MS.MovieService>> LoggerMock = new();
    protected readonly Mock<IMapper> MapperMock = new();

    protected MS.MovieService MovieService;

    private protected void InitService()
    {
        MovieService = new(
            MovieRepositoryMock.Object,
            LoggerMock.Object,
            MapperMock.Object);
    }

    private protected MovieDto[] ArrangeMovieDtos(int amount)
    {
        return new Faker<MovieDto>()
            .RuleFor(x => x.Title, y => y.Random.Words(3))
            .RuleFor(x => x.ReleaseDate, y => y.Date.Future(1))
            .RuleFor(x => x.GenreId, y => y.Random.Int(0, 1000))
            .RuleFor(x => x.DirectorId, y => y.Random.Int(0, 1000))
            .Generate(amount)
            .ToArray();
    }

    private protected Movie[] ArrangeMovieEntities(int amount)
    {
        return new Faker<Movie>()
            .RuleFor(x => x.Title, y => y.Random.Words(3))
            .RuleFor(x => x.ReleaseDate, y => y.Date.Future(1))
            .RuleFor(x => x.GenreId, y => y.Random.Int(0, 1000))
            .RuleFor(x => x.DirectorId, y => y.Random.Int(0, 1000))
            .Generate(amount)
            .ToArray();
    }

    private protected void SetDirectorId(int directorId, params MovieDto[] movies)
    {
        foreach (var movie in movies)
            movie.DirectorId = directorId;
    }
}
