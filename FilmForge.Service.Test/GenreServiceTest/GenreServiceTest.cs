namespace FilmForge.Service.Test.GenreServiceTest;

public class GenreServiceTest : BaseGenreServiceTest
{
    [Fact]
    public async Task CreateAsync_ShouldReturnGenreDto_WhenGenreIsCreated()
    {
        // Arrange
        var genreDto = ArrangeGenreDtos(1).FirstOrDefault();
        var genre = ArrangeGenreEntities(1).FirstOrDefault();

        GenreRepositoryMock
            .Setup(r => r.CreateAsync(genre, genreDto))
            .ReturnsAsync(genreDto);

        MapperMock
            .Setup(m => m.Map<Genre>(It.IsAny<GenreDto>()))
            .Returns(genre);

        LoggerMock
            .Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

        InitService();

        // Act
        var result = await GenreService.CreateAsync(genreDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<GenreDto>(result);
        GenreRepositoryMock.Verify(r => r.CreateAsync(genre, genreDto), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenGenreIsDeleted()
    {
        // Arrange
        int genreId = 1;
        GenreRepositoryMock
            .Setup(r => r.DeleteByIdAsync(genreId)).
            ReturnsAsync(true);

        InitService();

        // Act
        var result = await GenreService.DeleteByIdAsync(genreId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnArrayOfUserDtos()
    {
        // Arrange
        var genreDto = ArrangeGenreDtos(10);

        GenreRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(genreDto);

        InitService();

        // Act
        var result = await GenreService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(genreDto.Length, result.Length);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnGenreDto_WhenGenreExists()
    {
        // Arrange
        int genreId = 1;
        var genreDto = ArrangeGenreDtos(1).FirstOrDefault();

        GenreRepositoryMock
            .Setup(r => r.GetByIdAsync(genreId))
            .ReturnsAsync(genreDto);

        InitService();

        // Act
        var result = await GenreService.GetByIdAsync(genreId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<GenreDto>(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedGenreDto_WhenGenreIsUpdated()
    {
        // Arrange
        int genreId = 1;
        var genreDto = ArrangeGenreDtos(1).FirstOrDefault();
        var genre = ArrangeGenreEntities(1).FirstOrDefault();

        MapperMock
            .Setup(m => m.Map<Genre>(It.IsAny<GenreDto>()))
            .Returns(genre);

        GenreRepositoryMock
            .Setup(r => r.UpdateAsync(genreId, genre))
            .ReturnsAsync(genreDto);

        InitService();

        // Act
        var result = await GenreService.UpdateAsync(genreId, genreDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<GenreDto>(result);
        GenreRepositoryMock.Verify(r => r.UpdateAsync(genreId, genre), Times.Once);
    }
}
