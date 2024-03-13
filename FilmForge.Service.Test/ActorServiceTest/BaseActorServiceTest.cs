using FilmForge.Repository.ActorRepository;
using AS = FilmForge.Service.ActorService;

namespace FilmForge.Service.Test.ActorServiceTest;


public class BaseActorServiceTest
{
    protected readonly Mock<IActorRepository> ActorRepositoryMock = new();
    protected readonly Mock<ILogger<AS.ActorService>> LoggerMock = new();
    protected readonly Mock<IMapper> MapperMock = new();

    protected AS.ActorService ActorService;

    private protected void InitService()
    {
        ActorService = new(
            ActorRepositoryMock.Object,
            LoggerMock.Object,
            MapperMock.Object);
    }

    private protected ActorDto[] ArrangeActorDtos(int amount)
    {
        return new Faker<ActorDto>()
            .RuleFor(x => x.Name, y => y.Person.FullName)
            .RuleFor(x => x.Bio, y => y.Lorem.Sentence(20))
            .RuleFor(x => x.UserId, y => y.Random.Int(0, 1000))
            .RuleFor(x => x.Movies, y => new Faker<MovieDto>().Generate(3))
            .Generate(amount)
            .ToArray();
    }

    private protected Actor[] ArrangeActorEntities(int amount)
    {
        return new Faker<Actor>()
            .RuleFor(x => x.Name, y => y.Person.FullName)
            .RuleFor(x => x.Bio, y => y.Lorem.Sentence(20))
            .RuleFor(x => x.UserId, y => y.Random.Int(0, 1000))
            .RuleFor(x => x.Movies, y => new Faker<Movie>().Generate(3))
            .Generate(amount)
            .ToArray();
    }
}