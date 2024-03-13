using FilmForge.Common.Enum;
using FilmForge.Repository.UserRepository;
using US = FilmForge.Service.UserService;

namespace FilmForge.Service.Test.UserServiceTests;

public class BaseUserServiceTests
{
    protected readonly Mock<IUserRepository> UserRepositoryMock = new();
    protected readonly Mock<ILogger<US.UserService>> LoggerMock = new();
    protected readonly Mock<IMapper> MapperMock = new();

    protected US.UserService UserService;

    private protected void InitService()
    {
        UserService = new(
            UserRepositoryMock.Object,
            LoggerMock.Object,
            MapperMock.Object);
    }

    private protected UserDto[] ArrangeUserDtos(int amount)
    {
        return new Faker<UserDto>()
            .RuleFor(x => x.Name, y => y.Person.FullName)
            .RuleFor(x => x.Email, y => y.Internet.Email())
            .RuleFor(x => x.Role, y => y.PickRandom<UserRole>())
            .RuleFor(x => x.Password, y => y.Internet.Password())
            .RuleFor(x => x.Salt, y => string.Empty)
            .RuleFor(x => x.CreatedOn, y => y.Date.Past(1))
            .RuleFor(x => x.ModifiedOn, (y, x) => y.Date.Between(x.CreatedOn, DateTime.UtcNow))
            .Generate(amount)
            .ToArray();
    }

    private protected User[] ArrangeUserEntities(int amount)
    {
        return new Faker<User>()
            .RuleFor(x => x.Name, y => y.Person.FullName)
            .RuleFor(x => x.Email, y => y.Internet.Email())
            .RuleFor(x => x.Role, y => y.PickRandom<UserRole>())
            .RuleFor(x => x.Password, y => y.Random.Bytes(42))
            .RuleFor(x => x.Salt, y => y.Random.Bytes(42))
            .RuleFor(x => x.CreatedOn, y => y.Date.Past(1))
            .RuleFor(x => x.ModifiedOn, (y, x) => y.Date.Between(x.CreatedOn, DateTime.UtcNow))
            .Generate(amount)
            .ToArray();
    }
}
