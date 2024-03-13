using FilmForge.Repository.DirectorRepository;
using DS = FilmForge.Service.DirectorService;

namespace FilmForge.Service.Test.DirectorServiceTest
{
    public class BaseDirectorServiceTest
    {
        protected readonly Mock<IDirectorRepository> DirectorRepositoryMock = new();
        protected readonly Mock<ILogger<DS.DirectorService>> LoggerMock = new();
        protected readonly Mock<IMapper> MapperMock = new();

        protected DS.DirectorService DirectorService;

        private protected void InitService()
        {
            DirectorService = new(
                DirectorRepositoryMock.Object,
                LoggerMock.Object,
                MapperMock.Object);
        }

        private protected DirectorDto[] ArrangeDirectorDtos(int amount)
        {
            return new Faker<DirectorDto>()
                .RuleFor(x => x.Name, y => y.Person.FullName)
                .RuleFor(x => x.Bio, y => y.Lorem.Sentence(20))
                .RuleFor(x => x.UserId, y => y.Random.Int(0, 1000))
                .RuleFor(x => x.Movies, y => new Faker<MovieDto>().Generate(3))
                .Generate(amount)
                .ToArray();
        }

        private protected Director[] ArrangeDirectorEntities(int amount)
        {
            return new Faker<Director>()
                .RuleFor(x => x.Name, y => y.Person.FullName)
                .RuleFor(x => x.Bio, y => y.Lorem.Sentence(20))
                .RuleFor(x => x.UserId, y => y.Random.Int(0, 1000))
                .RuleFor(x => x.Movies, y => new Faker<Movie>().Generate(3))
                .Generate(amount)
                .ToArray();
        }
    }
}