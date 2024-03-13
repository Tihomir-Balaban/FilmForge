using FilmForge.Entities.EntityModels;

namespace FilmForge.Service.Test.ActorServiceTest;

public class ActorServiceTest : BaseActorServiceTest
{
    [Fact]
    public async Task CreateAsync_ShouldReturnActorDto_WhenActorIsCreated()
    {
        // Arrange
        var actorDto = ArrangeActorDtos(1).FirstOrDefault();
        var actor = ArrangeActorEntities(1).FirstOrDefault();

        ActorRepositoryMock
            .Setup(r => r.CreateAsync(actor, actorDto))
            .ReturnsAsync(actorDto);

        MapperMock
            .Setup(m => m.Map<Actor>(It.IsAny<ActorDto>()))
            .Returns(actor);

        LoggerMock
            .Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

        InitService();

        // Act
        var result = await ActorService.CreateAsync(actorDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ActorDto>(result);
        ActorRepositoryMock.Verify(r => r.CreateAsync(actor, actorDto), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenActorIsDeleted()
    {
        // Arrange
        int actorId = 1;
        ActorRepositoryMock
            .Setup(r => r.DeleteByIdAsync(actorId)).
            ReturnsAsync(true);

        InitService();

        // Act
        var result = await ActorService.DeleteByIdAsync(actorId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnArrayOfUserDtos()
    {
        // Arrange
        var actorDto = ArrangeActorDtos(10);

        ActorRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(actorDto);

        InitService();

        // Act
        var result = await ActorService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(actorDto.Length, result.Length);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnActorDto_WhenActorExists()
    {
        // Arrange
        int actorId = 1;
        var actorDto = ArrangeActorDtos(1).FirstOrDefault();

        ActorRepositoryMock
            .Setup(r => r.GetByIdAsync(actorId))
            .ReturnsAsync(actorDto);

        InitService();

        // Act
        var result = await ActorService.GetByIdAsync(actorId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ActorDto>(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedActorDto_WhenActorIsUpdated()
    {
        // Arrange
        int actorId = 1;
        var actorDto = ArrangeActorDtos(1).FirstOrDefault();
        var actor = ArrangeActorEntities(1).FirstOrDefault();

        MapperMock
            .Setup(m => m.Map<Actor>(It.IsAny<ActorDto>()))
            .Returns(actor);

        ActorRepositoryMock
            .Setup(r => r.UpdateAsync(actorId, actor))
            .ReturnsAsync(actorDto);

        InitService();

        // Act
        var result = await ActorService.UpdateAsync(actorId, actorDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ActorDto>(result);
        ActorRepositoryMock.Verify(r => r.UpdateAsync(actorId, actor), Times.Once);
    }
}