using FilmForge.Repository.ReviewRepository;
using ReS = FilmForge.Service.ReviewService;

namespace FilmForge.Service.Test.ReviewServiceTest;

public class BaseReviewServiceTest
{
    protected readonly Mock<IReviewRepository> ReviewRepositoryMock = new();
    protected readonly Mock<ILogger<ReS.ReviewService>> LoggerMock = new();
    protected readonly Mock<IMapper> MapperMock = new();

    protected ReS.ReviewService ReviewService;

    private protected void InitService()
    {
        ReviewService = new(
            ReviewRepositoryMock.Object,
            LoggerMock.Object,
            MapperMock.Object);
    }

    private protected ReviewDto[] ArrangeReviewDtos(int amount)
    {
        return new Faker<ReviewDto>()
            .RuleFor(x => x.Title, y => y.Lorem.Sentence(4))
            .RuleFor(x => x.Content, y => y.Lorem.Sentence(60))
            .RuleFor(x => x.UserId, y => y.Random.Int(0, 1000))
            .RuleFor(x => x.MovieId, y => y.Random.Int(0, 1000))
            .Generate(amount)
            .ToArray();
    }

    private protected Review[] ArrangeReviewEntities(int amount)
    {
        return new Faker<Review>()
            .RuleFor(x => x.Title, y => y.Lorem.Sentence(4))
            .RuleFor(x => x.Content, y => y.Lorem.Sentence(60))
            .RuleFor(x => x.UserId, y => y.Random.Int(0, 1000))
            .RuleFor(x => x.MovieId, y => y.Random.Int(0, 1000))
            .Generate(amount)
            .ToArray();
    }
}
