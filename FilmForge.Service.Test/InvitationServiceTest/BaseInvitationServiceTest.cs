using FilmForge.Common.Enum;
using FilmForge.Repository.InvitationRepository;
using FilmForge.Service.ActorService;
using FilmForge.Service.MovieService;
using IS = FilmForge.Service.InvitationService;

namespace FilmForge.Service.Test.InvitationServiceTest;

public class BaseInvitationServiceTest
{
    protected readonly Mock<IInvitationRepository> InvitationRepositoryMock = new();
    protected readonly Mock<IMovieService> MovieServiceMock = new();
    protected readonly Mock<IActorService> ActorServiceMock = new();
    protected readonly Mock<ILogger<IS.InvitationService>> LoggerMock = new();
    protected readonly Mock<IMapper> MapperMock = new();

    protected IS.InvitationService InvitationService;

    private protected void InitService()
    {
        InvitationService = new(
            InvitationRepositoryMock.Object,
            MovieServiceMock.Object,
            ActorServiceMock.Object,
            LoggerMock.Object,
            MapperMock.Object);
    }

    private protected InvitationDto[] ArrangInvitationDtos(int amount)
    {
        return new Faker<InvitationDto>()
            .RuleFor(x => x.CreatedOn, y => DateTime.Now)
            .RuleFor(x => x.ModifiedOn, y => DateTime.Now)
            .RuleFor(x => x.HasAccepted, y => false)
            .RuleFor(x => x.InvitationType, y => y.PickRandom<InvitationType>())
            .RuleFor(x => x.MovieId, y => y.Random.Int(0, 1000))
            .RuleFor(x => x.ActorId, y => y.Random.Int(0, 1000))
            .Generate(amount)
            .ToArray();
    }

    private protected Invitation[] ArrangeInvitationEntities(int amount)
    {
        return new Faker<Invitation>()
            .RuleFor(x => x.Id, y => y.Random.Int(0, 1000))
            .RuleFor(x => x.CreatedOn, y => DateTime.Now)
            .RuleFor(x => x.ModifiedOn, y => DateTime.Now)
            .RuleFor(x => x.HasAccepted, y => false)
            .RuleFor(x => x.InvitationType, y => y.PickRandom<InvitationType>())
            .RuleFor(x => x.MovieId, y => y.Random.Int(0, 1000))
            .RuleFor(x => x.ActorId, y => y.Random.Int(0, 1000))
            .Generate(amount)
            .ToArray();
    }
}