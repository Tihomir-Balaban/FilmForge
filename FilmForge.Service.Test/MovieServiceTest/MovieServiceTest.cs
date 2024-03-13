namespace FilmForge.Service.Test.MovieServiceTest;

public class MovieServiceTest : BaseMovieServiceTest
{
    [Fact]
    public async Task CreateAsync_ShouldReturnMovieDto_WhenMovieIsCreated()
    {
        // Arrange
        var movieDto = ArrangeMovieDtos(1).FirstOrDefault();
        var movie = ArrangeMovieEntities(1).FirstOrDefault();

        MovieRepositoryMock
            .Setup(r => r.CreateAsync(movie, movieDto))
            .ReturnsAsync(movieDto);

        MapperMock
            .Setup(m => m.Map<Movie>(It.IsAny<MovieDto>()))
            .Returns(movie);

        LoggerMock
            .Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

        InitService();

        // Act
        var result = await MovieService.CreateAsync(movieDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MovieDto>(result);
        MovieRepositoryMock.Verify(r => r.CreateAsync(movie, movieDto), Times.Once);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenMovieIsDeleted()
    {
        // Arrange
        int movieId = 1;
        MovieRepositoryMock
            .Setup(r => r.DeleteByIdAsync(movieId)).
            ReturnsAsync(true);

        InitService();

        // Act
        var result = await MovieService.DeleteByIdAsync(movieId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnArrayOfUserDtos()
    {
        // Arrange
        var movieDto = ArrangeMovieDtos(10);

        MovieRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(movieDto);

        InitService();

        // Act
        var result = await MovieService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(movieDto.Length, result.Length);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMovieDto_WhenMovieExists()
    {
        // Arrange
        int movieId = 1;
        var movieDto = ArrangeMovieDtos(1).FirstOrDefault();

        MovieRepositoryMock
            .Setup(r => r.GetByIdAsync(movieId))
            .ReturnsAsync(movieDto);

        InitService();

        // Act
        var result = await MovieService.GetByIdAsync(movieId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MovieDto>(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedMovieDto_WhenMovieIsUpdated()
    {
        // Arrange
        int movieId = 1;
        var movieDto = ArrangeMovieDtos(1).FirstOrDefault();
        var movie = ArrangeMovieEntities(1).FirstOrDefault();

        MapperMock
            .Setup(m => m.Map<Movie>(It.IsAny<MovieDto>()))
            .Returns(movie);

        MovieRepositoryMock
            .Setup(r => r.UpdateAsync(movieId, movie))
            .ReturnsAsync(movieDto);

        InitService();

        // Act
        var result = await MovieService.UpdateAsync(movieId, movieDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MovieDto>(result);
        MovieRepositoryMock.Verify(r => r.UpdateAsync(movieId, movie), Times.Once);
    }
}
