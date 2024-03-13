namespace FilmForge.Service.Test.DirectorServiceTest;

public class DirectorServiceTest : BaseDirectorServiceTest
{
    [Fact]
    public async Task CreateAsync_ShouldReturnDirectorDto_WhenDirectorIsCreated()
    {
        // Arrange
        var directorDto = ArrangeDirectorDtos(1).FirstOrDefault();
        var director = ArrangeDirectorEntities(1).FirstOrDefault();

        DirectorRepositoryMock
            .Setup(r => r.CreateAsync(director, directorDto))
            .ReturnsAsync(directorDto);

        MapperMock
            .Setup(m => m.Map<Director>(It.IsAny<DirectorDto>()))
            .Returns(director);

        LoggerMock
            .Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

        InitService();

        // Act
        var result = await DirectorService.CreateAsync(directorDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<DirectorDto>(result);
        DirectorRepositoryMock.Verify(r => r.CreateAsync(director, directorDto), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenDirectorIsDeleted()
    {
        // Arrange
        int directorId = 1;
        DirectorRepositoryMock
            .Setup(r => r.DeleteByIdAsync(directorId)).
            ReturnsAsync(true);

        InitService();

        // Act
        var result = await DirectorService.DeleteByIdAsync(directorId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnArrayOfUserDtos()
    {
        // Arrange
        var directorDto = ArrangeDirectorDtos(10);

        DirectorRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(directorDto);

        InitService();

        // Act
        var result = await DirectorService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(directorDto.Length, result.Length);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDirectorDto_WhenDirectorExists()
    {
        // Arrange
        int directorId = 1;
        var directorDto = ArrangeDirectorDtos(1).FirstOrDefault();

        DirectorRepositoryMock
            .Setup(r => r.GetByIdAsync(directorId))
            .ReturnsAsync(directorDto);

        InitService();

        // Act
        var result = await DirectorService.GetByIdAsync(directorId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<DirectorDto>(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedDirectorDto_WhenDirectorIsUpdated()
    {
        // Arrange
        int directorId = 1;
        var directorDto = ArrangeDirectorDtos(1).FirstOrDefault();
        var director = ArrangeDirectorEntities(1).FirstOrDefault();

        MapperMock
            .Setup(m => m.Map<Director>(It.IsAny<DirectorDto>()))
            .Returns(director);

        DirectorRepositoryMock
            .Setup(r => r.UpdateAsync(directorId, director))
            .ReturnsAsync(directorDto);

        InitService();

        // Act
        var result = await DirectorService.UpdateAsync(directorId, directorDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<DirectorDto>(result);
        DirectorRepositoryMock.Verify(r => r.UpdateAsync(directorId, director), Times.Once);
    }
}