using FilmForge.Entities.EntityModels;
using FilmForge.Repository.ActorRepository;
using Microsoft.EntityFrameworkCore;
using AR = FilmForge.Repository.ActorRepository;

namespace FilmForge.Repository.Intergration.Tests.ActorRepositoryTests;

public class ActorRepositoryIntergrationTests : BaseActorRepositoryIntergrationTests<AR.ActorRepository>
{

    private readonly AR.ActorRepository actorRepository;

    public ActorRepositoryIntergrationTests()
	{
        actorRepository = new(
            filmForgeDbContext,
            loggerMock.Object,
            mapper);
	}

    [Fact]
    public async Task CreateAsync_ShouldAddActor()
    {
        // Arrange
        var actor = new Faker<Actor>()
                .RuleFor(x => x.CreatedOn, y => DateTime.Now)
                .RuleFor(x => x.ModifiedOn, y => DateTime.Now)
                .RuleFor(x => x.Name, y => y.Person.FullName)
                .RuleFor(x => x.Bio, y => y.Lorem.Sentence(20))
                .RuleFor(x => x.Fee, y => y.Random.ULong())
                .RuleFor(x => x.UserId, y => users.First().Id)
                .Generate();

        var actorDto = mapper.Map<ActorDto>(actor);

        // Act
        var result = await actorRepository.CreateAsync(actor, actorDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(actor.Name, result.Name);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldRemoveActor()
    {
        // Arrange
        var actor = new Faker<Actor>()
                .RuleFor(x => x.CreatedOn, y => DateTime.Now)
                .RuleFor(x => x.ModifiedOn, y => DateTime.Now)
                .RuleFor(x => x.Name, y => y.Person.FullName)
                .RuleFor(x => x.Bio, y => y.Lorem.Sentence(20))
                .RuleFor(x => x.Fee, y => y.Random.ULong())
                .RuleFor(x => x.UserId, y => users.First().Id)
                .Generate();

        var actorDto = mapper.Map<ActorDto>(actor);

        var actorToBeDeleted = await actorRepository.CreateAsync(actor, actorDto);

        // Act
        var result = await actorRepository.DeleteByIdAsync(actorToBeDeleted.Id);

        // Assert
        Assert.True(result);
        Assert.Null(await filmForgeDbContext.Actors.FindAsync(actorToBeDeleted.Id));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnActors()
    {
        // Arrange
        // Act
        var result = await actorRepository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnActor()
    {
        // Arrange
        var actor = actors.First();
        Assert.NotNull(actor);

        // Act
        var result = await actorRepository.GetByIdAsync(actor.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(actor.Id, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyActor()
    {
        // Arrange
        var actor = await filmForgeDbContext.Actors.FirstOrDefaultAsync();
        Assert.NotNull(actor);

        var updatedActor = new Faker<Actor>()
                .RuleFor(x => x.Id, y => actor.Id)
                .RuleFor(x => x.CreatedOn, y => DateTime.Now)
                .RuleFor(x => x.ModifiedOn, y => DateTime.Now)
                .RuleFor(x => x.Name, y => y.Person.FullName)
                .RuleFor(x => x.Bio, y => y.Lorem.Sentence(20))
                .RuleFor(x => x.Fee, y => y.Random.ULong())
                .RuleFor(x => x.UserId, y => actor.UserId)
                .Generate();

        // Act
        var result = await actorRepository.UpdateAsync(actor.Id, updatedActor);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(actor.Name, result.Name);
    }
}