using FilmForge.Repository.RatingRepository;
using RaS = FilmForge.Service.RatingService;

namespace FilmForge.Service.Test.RatingServiceTest;

public class BaseRatingServiceTest
{
    protected readonly Mock<IRatingRepository> RatingRepositoryMock = new();
    protected readonly Mock<ILogger<RaS.RatingService>> LoggerMock = new();
    protected readonly Mock<IMapper> MapperMock = new();

    protected RaS.RatingService RatingService;

    private protected void InitService()
    {
        RatingService = new(
            RatingRepositoryMock.Object,
            LoggerMock.Object,
            MapperMock.Object);
    }

    private protected RatingDto[] ArrangeRatingDtos(int amount)
    {
        return new Faker<RatingDto>()
            .RuleFor(x => x.Value, y => y.Random.Int(1, 5))
            .RuleFor(x => x.MovieId, y => y.Random.Int(1, 1000))
            .RuleFor(x => x.UserId, y => y.Random.Int(1, 1000))
            .Generate(amount)
            .ToArray();
    }

    private protected Rating[] ArrangeRatingEntities(int amount)
    {
        return new Faker<Rating>()
            .RuleFor(x => x.Value, y => y.Random.Int(1, 5))
            .RuleFor(x => x.MovieId, y => y.Random.Int(1, 1000))
            .RuleFor(x => x.UserId, y => y.Random.Int(1, 1000))
            .Generate(amount)
            .ToArray();
    }
}
